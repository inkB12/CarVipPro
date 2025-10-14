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
            var resultDto = new DriveScheduleViewDto
            {
                Id = schedule.Id,
                VehicleModel = schedule.ElectricVehicle?.Model ?? string.Empty,
                CustomerName = customer?.FullName ?? string.Empty,
                StaffName = staff?.FullName ?? string.Empty,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Status = schedule.Status
            };

            return (true, "Đặt lịch lái thử thành công!", resultDto);
        }
    }
}
