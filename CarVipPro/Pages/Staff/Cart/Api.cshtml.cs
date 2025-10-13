using CarVipPro.APrenstationLayer.Hubs;
using CarVipPro.APrenstationLayer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace CarVipPro.APrenstationLayer.Pages.Staff.Cart
{
    [IgnoreAntiforgeryToken]
    public class ApiModel : PageModel
    {
        private readonly IHubContext<CartHub> _hub;
        public ApiModel(IHubContext<CartHub> hub)
        {
            _hub = hub;
        }

        public IActionResult OnGetItems()
        {
            var cart = HttpContext.Session.GetCart();
            var items = cart.Items.Select(i => new {
                id = i.ElectricVehicleId,
                name = i.Name,
                unitPrice = i.UnitPrice,
                qty = i.Quantity,
                lineTotal = i.LineTotal,
                imageUrl = i.ImageUrl,
                color = i.Color
            }).ToList();
            return new JsonResult(new { items, total = cart.Total, count = cart.Items.Sum(x => x.Quantity) });
        }

        public async Task<IActionResult> OnPostUpdate([FromBody] UpdateReq req)
        {
            if (req == null || req.Id <= 0) return new JsonResult(new { ok = false });
            var cart = HttpContext.Session.GetCart();
            var it = cart.Items.FirstOrDefault(x => x.ElectricVehicleId == req.Id);
            if (it == null) return new JsonResult(new { ok = false });

            it.Quantity = Math.Max(1, req.Qty);
            HttpContext.Session.SaveCart(cart);

            var channel = CartChannel.EnsureChannel(HttpContext.Session);
            var count = cart.Items.Sum(x => x.Quantity);
            await _hub.Clients.Group(channel).SendAsync("CartUpdated", count);

            return new JsonResult(new { ok = true, total = cart.Total, count });
        }

        public async Task<IActionResult> OnPostRemove([FromBody] IdReq req)
        {
            if (req == null || req.Id <= 0) return new JsonResult(new { ok = false });
            var cart = HttpContext.Session.GetCart();
            cart.Items.RemoveAll(x => x.ElectricVehicleId == req.Id);
            HttpContext.Session.SaveCart(cart);

            var channel = CartChannel.EnsureChannel(HttpContext.Session);
            var count = cart.Items.Sum(x => x.Quantity);
            await _hub.Clients.Group(channel).SendAsync("CartUpdated", count);

            return new JsonResult(new { ok = true, total = cart.Total, count });
        }

        public async Task<IActionResult> OnPostClear()
        {
            HttpContext.Session.ClearCart();
            var channel = CartChannel.EnsureChannel(HttpContext.Session);
            await _hub.Clients.Group(channel).SendAsync("CartUpdated", 0);
            return new JsonResult(new { ok = true, total = 0, count = 0 });
        }

        public record IdReq(int Id);
        public record UpdateReq(int Id, int Qty);
    }
}
