using System.Security.Claims;
using CarVipPro.APrenstationLayer.Hubs;
using CarVipPro.APrenstationLayer.Infrastructure;
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace CarVipPro.APrenstationLayer.Pages.Staff.DriveTest
{
    public class EditScheduleModel : PageModel
    {
        private readonly IDriveScheduleService _driveScheduleService;
        private readonly IHubContext<NotifyHub> _hubContext;

        [BindProperty]
        public DriveScheduleCreateDto Schedule { get; set; } = new();
        [BindProperty]
        public DateTime? SelectedDate { get; set; }
        public string? Message { get; set; }

        public string MainLayout { get; set; }

        public EditScheduleModel(IDriveScheduleService driveScheduleService, IHubContext<NotifyHub> hubContext)
        {
            _driveScheduleService = driveScheduleService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGetAsync(int scheduleId)
        {
            string? role = HttpContext.Session.GetString(SessionKeys.Role);

            MainLayout = "_Layout";

            if (role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
            {
                MainLayout = "_LayoutAdmin";
            }

            Schedule = await _driveScheduleService.GetScheduleByIdAsync(scheduleId);

            if (Schedule == null)
            {
                return RedirectToPage("Schedules");
            }

            SelectedDate = Schedule.StartTime.Date;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Vui lòng kiểm tra lại các trường thông tin.";
                return Page();
            }

            // Gộp SelectedDate với giờ đã chọn
            Schedule.StartTime = SelectedDate.Value.Date
                .Add(TimeSpan.Parse(Schedule.StartTime.ToString("HH:mm")));
            Schedule.EndTime = SelectedDate.Value.Date
                .Add(TimeSpan.Parse(Schedule.EndTime.ToString("HH:mm")));

            var result = await _driveScheduleService.UpdateSchedule(Schedule, Schedule.Id);

            Message = result.Message;

            if (result.Success)
            {
                await _hubContext.Clients.Group("ScheduleUpdates")
                    .SendAsync("ReceiveScheduleUpdate", result.UpdatedSchedule);
                return RedirectToPage("Schedules");
            }

            return Page();
        }
    }
}
