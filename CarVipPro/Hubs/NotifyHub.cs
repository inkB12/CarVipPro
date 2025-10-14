using Microsoft.AspNetCore.SignalR;

namespace CarVipPro.APrenstationLayer.Hubs
{
    public class NotifyHub : Hub
    {
        // Ví dụ khi lịch lái thử được cập nhật trạng thái
        public async Task BroadcastDriveScheduleStatus(int scheduleId, string newStatus)
        {
            await Clients.All.SendAsync("ReceiveDriveScheduleStatus", scheduleId, newStatus);
        }
    }
}
