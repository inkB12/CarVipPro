using Microsoft.AspNetCore.SignalR;

namespace CarVipPro.APrenstationLayer.Hubs
{
    public class CartHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var http = Context.GetHttpContext();
            var channel = http?.Request.Query["channel"].ToString();

            if (!string.IsNullOrWhiteSpace(channel))
                await Groups.AddToGroupAsync(Context.ConnectionId, channel);

            await base.OnConnectedAsync();
        }
    }
}
