using System.Security.Claims;
using CarVipPro.APrenstationLayer.Infrastructure;
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.DriveTest
{
    public class SchedulesModel : PageModel
    {
        private readonly IDriveScheduleService _driveScheduleService;
        private readonly IElectricVehicleService _electricVehicleService;

        public SchedulesModel(IDriveScheduleService driveScheduleService, IElectricVehicleService electricVehicleService)
        {
            _driveScheduleService = driveScheduleService;
            _electricVehicleService = electricVehicleService;
        }

        public List<DriveScheduleViewDto> Schedules { get; set; } = [];
        public IEnumerable<ElectricVehicleDTO> Vehicles { get; set; } = [];

        // Filter -- Apply for Query String
        [BindProperty(SupportsGet = true)]
        public DateTime FilterDate { get; set; } = default;

        // Filter -- Apply for Query String
        [BindProperty(SupportsGet = true)]
        public int VehicleId { get; set; }

        public string MainLayout { get; set; }

        // Call when user access the page
        public async Task OnGetAsync()
        {
            string? role = HttpContext.Session.GetString(SessionKeys.Role);

            MainLayout = "_LayoutStaff";

            if (role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                MainLayout = "_LayoutAdmin";
            }


            Vehicles = await _electricVehicleService.GetAll();
            Schedules = await _driveScheduleService.GetSchedulesByDateAsync(vehicleId: VehicleId, date: FilterDate);
        }

        public async Task<IActionResult> OnGetScheduleRowPartial(int scheduleId)
        {
            // 1. Lấy dữ liệu lịch lái thử mới nhất
            var schedule = await _driveScheduleService.GetViewScheduleByIdAsync(scheduleId);

            if (schedule == null)
            {
                return new NotFoundResult();
            }

            // 2. Truyền DTO vào Partial View
            return Partial("_ScheduleRowPartial", schedule);
        }
    }
}
