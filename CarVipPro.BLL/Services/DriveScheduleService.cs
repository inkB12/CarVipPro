using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarVipPro.BLL.Services
{
    public class DriveScheduleService : IDriveScheduleService
    {
        private readonly IDriveScheduleRepository _driveRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IEmailService _emailService;

        public DriveScheduleService(
            IDriveScheduleRepository driveRepo,
            ICustomerRepository customerRepo,
            IAccountRepository accountRepo,
            IEmailService emailService)
        {
            _driveRepo = driveRepo;
            _customerRepo = customerRepo;
            _accountRepo = accountRepo;
            _emailService = emailService;
        }

        // 📅 Lấy tất cả lịch trong ngày của xe
        public async Task<List<DriveScheduleViewDto>> GetSchedulesByVehicleAndDateAsync(int vehicleId, DateTime date)
        {
            var schedules = await _driveRepo.GetSchedulesByVehicleAndDateAsync(vehicleId, date);

            return schedules
                .Select(s => new DriveScheduleViewDto
                {
                    Id = s.Id,
                    VehicleModel = s.ElectricVehicle?.Model ?? "N/A",
                    CustomerName = s.Customer?.FullName ?? "N/A",
                    StaffName = s.Account?.FullName ?? "N/A",
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Status = s.Status
                })
                .ToList();
        }

        // 📅 Lấy tất cả lịch trong ngày của xe (Dynamic Searching)
        public async Task<List<DriveScheduleViewDto>> GetSchedulesByDateAsync(int vehicleId, DateTime date = default)
        {
            var schedules = await _driveRepo.GetSchedulesByDateAsync(vehicleId, date);

            return [.. schedules
                .Select(s => new DriveScheduleViewDto
                {
                    Id = s.Id,
                    VehicleModel = s.ElectricVehicle?.Model ?? "N/A",
                    CustomerName = s.Customer?.FullName ?? "N/A",
                    StaffName = s.Account?.FullName ?? "N/A",
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Status = s.Status
                })];
        }

        // ➕ Tạo mới lịch lái thử
        public async Task<(bool Success, string Message, DriveScheduleViewDto? CreatedSchedule)> CreateAsync(DriveScheduleCreateDto dto)
        {
            // 1️⃣ Kiểm tra hợp lệ thời gian
            if (dto.EndTime <= dto.StartTime)
                return (false, "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.", null);

            var duration = dto.EndTime - dto.StartTime;
            if (duration.TotalMinutes < 15)
                return (false, "Thời lượng lái thử tối thiểu là 15 phút.", null);

            // 2️⃣ Lấy danh sách lịch đã có trong ngày của xe
            var sameDaySchedules = await _driveRepo.GetSchedulesByVehicleAndDateAsync(dto.ElectricVehicleId, dto.StartTime);

            // 3️⃣ Kiểm tra trùng giờ
            foreach (var s in sameDaySchedules)
            {
                if (dto.StartTime < s.EndTime && dto.EndTime > s.StartTime)
                {
                    return (false, $"Xe đã có lịch từ {s.StartTime:HH:mm} đến {s.EndTime:HH:mm}.", null);
                }
            }

            // 4️⃣ Lưu lịch mới
            var schedule = new DriveSchedule
            {
                ElectricVehicleId = dto.ElectricVehicleId,
                CustomerId = dto.CustomerId,
                AccountId = dto.AccountId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = "COMING_SOON"
            };

            await _driveRepo.AddAsync(schedule);

            // 5️⃣ Gửi email xác nhận
            var customer = await _customerRepo.GetByIdWithDriveSchedulesAsync(dto.CustomerId);
            var staff = await _accountRepo.GetByIdAsync(dto.AccountId);
            if (customer != null && staff != null)
            {
                string subject = "Xác nhận lịch lái thử CarVipPro";
                string body = $@"
                    <p>Xin chào {customer.FullName},</p>
                    <p>Bạn đã có lịch lái thử xe với CarVipPro:</p>
                    <ul>
                        <li>Xe: {schedule.ElectricVehicle?.Model}</li>
                        <li>Thời gian: {schedule.StartTime:HH:mm} - {schedule.EndTime:HH:mm} ngày {schedule.StartTime:dd/MM/yyyy}</li>
                        <li>Nhân viên phụ trách: {staff.FullName}</li>
                    </ul>
                    <p>Chúng tôi rất mong được đón tiếp bạn tại showroom!</p>";

                await _emailService.SendAsync(customer.Email, subject, body);
            }

            // 6️⃣ Trả lại dữ liệu để Presentation broadcast SignalR

            return (true, "Đặt lịch lái thử thành công!", MapToDTO(schedule));
        }

        public async Task<(bool Success, string Message, DriveScheduleViewDto? UpdatedSchedule)> UpdateSchedule(DriveScheduleCreateDto dto, int? driveScheduleId = 0)
        {
            var entity = _driveRepo.GetDriveScheduleByIdAsync(driveScheduleId).Result;
            if (entity == null) return (false, "Không tìm thấy lịch lái thử.", null);

            entity.Status = dto.Status == default ? entity.Status : dto.Status;

            if (dto.StartTime != entity.StartTime || dto.EndTime != entity.EndTime)
            {
                var (result, message) = IsValid(dto);
                if (!result)
                {
                    return (false, message, null);
                }

                bool isOverlap = await IsOverlappTime(dto.StartTime, dto.EndTime, dto.ElectricVehicleId);

                if (isOverlap)
                {
                    return (false, $"Xe đã có lịch từ {dto.StartTime:HH:mm} đến {dto.EndTime:HH:mm}.", null);
                }

                entity.StartTime = dto.StartTime;
                entity.EndTime = dto.EndTime;
            }

            await _driveRepo.UpdateAsync(entity);

            return (true, "Đặt lịch lái thử thành công!", MapToDTO(entity));
        }

        private DriveScheduleViewDto MapToDTO(DriveSchedule schedule)
        {
            return new DriveScheduleViewDto
            {
                Id = schedule.Id,
                VehicleModel = schedule.ElectricVehicle?.Model ?? string.Empty,
                CustomerName = schedule.Customer.FullName ?? string.Empty,
                StaffName = schedule.Account.FullName ?? string.Empty,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Status = schedule.Status
            };
        }

        private DriveScheduleCreateDto MapToCreateDTO(DriveSchedule schedule)
        {
            return new DriveScheduleCreateDto
            {
                Id = schedule.Id,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Status = schedule.Status
            };
        }

        private async Task<bool> IsOverlappTime(DateTime start, DateTime end, int vehicleId)
        {
            // 2️⃣ Lấy danh sách lịch đã có trong ngày của xe
            var sameDaySchedules = await _driveRepo.GetSchedulesByVehicleAndDateAsync(vehicleId, start.Date);

            // 3️⃣ Kiểm tra trùng giờ
            foreach (var s in sameDaySchedules)
            {
                if (start < s.EndTime && end > s.StartTime)
                {
                    return true;
                }
            }

            return false;
        }

        private (bool result, string message) IsValid(DriveScheduleCreateDto dto)
        {
            // Kiểm tra hợp lệ thời gian
            if (dto.StartTime < DateTime.Now)
                return (false, "Thời gian không thể là quá khứ");

            if (dto.EndTime <= dto.StartTime)
                return (false, "Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");

            var duration = dto.EndTime - dto.StartTime;
            if (duration.TotalMinutes < 15)
                return (false, "Thời lượng lái thử tối thiểu là 15 phút.");

            return (true, "");
        }

        public async Task<DriveScheduleCreateDto> GetScheduleByIdAsync(int scheduleId = 0)
        {
            return MapToCreateDTO(await _driveRepo.GetDriveScheduleByIdAsync(scheduleId));
        }

        public async Task<DriveScheduleViewDto> GetViewScheduleByIdAsync(int scheduleId = 0)
        {
            return MapToDTO(await _driveRepo.GetDriveScheduleByIdAsync(scheduleId));
        }
    }
}
