using CarVipPro.APrenstationLayer.Hubs;
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace CarVipPro.APrenstationLayer.Pages.Staff.DriveTest
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IDriveScheduleService _driveScheduleService;
        private readonly ICarCompanyService _carCompanyService;
        private readonly IElectricVehicleService _vehicleService;
        private readonly IHubContext<NotifyHub> _hubContext;

        public CreateModel(
            ICustomerService customerService,
            IDriveScheduleService driveScheduleService,
            ICarCompanyService carCompanyService,
            IElectricVehicleService vehicleService,
            IHubContext<NotifyHub> hubContext)
        {
            _customerService = customerService;
            _driveScheduleService = driveScheduleService;
            _carCompanyService = carCompanyService;
            _vehicleService = vehicleService;
            _hubContext = hubContext;
        }

        [BindProperty(SupportsGet = true)]
        public int CustomerId { get; set; }

        public CustomerDto? Customer { get; set; }
        public List<CarCompanyDto> CarCompanies { get; set; } = new();
        public List<ElectricVehicleDto> Vehicles { get; set; } = new();
        public List<DriveScheduleViewDto> ExistingSchedules { get; set; } = new();

        [BindProperty]
        public DriveScheduleCreateDto CreateDto { get; set; } = new();

        [BindProperty]
        public int? SelectedCompanyId { get; set; }

        [BindProperty]
        public int? SelectedVehicleId { get; set; }

        [BindProperty]
        public DateTime? SelectedDate { get; set; }

        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            Customer = await _customerService.GetByIdAsync(CustomerId);
            CarCompanies = await _carCompanyService.GetActiveCompaniesAsync();

            // load xe mặc định của hãng đầu tiên
            if (CarCompanies.Count > 0)
            {
                var firstCompanyId = SelectedCompanyId ?? CarCompanies.First().Id;
                Vehicles = await _vehicleService.GetActiveByCompanyAsync(firstCompanyId);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Customer = await _customerService.GetByIdAsync(CustomerId);
            CarCompanies = await _carCompanyService.GetActiveCompaniesAsync();

            if (SelectedCompanyId.HasValue)
                Vehicles = await _vehicleService.GetActiveByCompanyAsync(SelectedCompanyId.Value);

            // ✅ VALIDATION: kiểm tra ngày
            if (!SelectedDate.HasValue || SelectedDate.Value.Date < DateTime.Today)
            {
                Message = "❌ Ngày lái thử phải là hôm nay hoặc trong tương lai.";
                return Page();
            }

            if (!SelectedVehicleId.HasValue)
            {
                Message = "❌ Vui lòng chọn xe.";
                return Page();
            }

            // Gộp SelectedDate với giờ đã chọn
            CreateDto.StartTime = SelectedDate.Value.Date
                .Add(TimeSpan.Parse(CreateDto.StartTime.ToString("HH:mm")));
            CreateDto.EndTime = SelectedDate.Value.Date
                .Add(TimeSpan.Parse(CreateDto.EndTime.ToString("HH:mm")));

            CreateDto.CustomerId = CustomerId;
            CreateDto.ElectricVehicleId = SelectedVehicleId.Value;
            CreateDto.AccountId = 1; // staff login giả định

            var result = await _driveScheduleService.CreateAsync(CreateDto);

            if (!result.Success)
            {
                Message = result.Message;
                return Page();
            }

            await _hubContext.Clients.All.SendAsync("ReceiveDriveScheduleAdded", new
            {
                VehicleId = CreateDto.ElectricVehicleId,
                Date = CreateDto.StartTime.Date,
                StartTime = CreateDto.StartTime,
                EndTime = CreateDto.EndTime,
                Customer = result.CreatedSchedule?.CustomerName ?? "Không rõ"
            });

            TempData["SuccessMessage"] = result.Message;
            return RedirectToPage("/Staff/DriveTest/Create", new { customerId = CustomerId });
        }
        // ✅ Load danh sách lịch (partial AJAX)
        public async Task<PartialViewResult> OnGetSchedulePartialAsync(int vehicleId, DateTime date)
        {
            var schedules = await _driveScheduleService.GetSchedulesByVehicleAndDateAsync(vehicleId, date);
            return Partial("_ScheduleListPartial", schedules);
        }
    }
}

