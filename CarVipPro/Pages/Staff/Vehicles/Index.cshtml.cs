namespace CarVipPro.APrenstationLayer.Pages.Staff.Vehicles
{
    using CarVipPro.APrenstationLayer.Infrastructure;
    using CarVipPro.APrenstationLayer.Hubs;
    using CarVipPro.BLL.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.SignalR;

    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IElectricVehicleService _evSvc;
        private readonly ICarCompanyService _companySvc;
        private readonly IVehicleCategoryService _categorySvc;
        private readonly IHubContext<CartHub> _hub;

        public IndexModel(IElectricVehicleService evSvc,
                          ICarCompanyService companySvc,
                          IVehicleCategoryService categorySvc,
                          IHubContext<CartHub> hub)
        {
            _evSvc = evSvc;
            _companySvc = companySvc;
            _categorySvc = categorySvc;
            _hub = hub;
        }

        [BindProperty(SupportsGet = true)] public string? Q { get; set; }
        [BindProperty(SupportsGet = true)] public int? CompanyId { get; set; }
        [BindProperty(SupportsGet = true)] public int? CategoryId { get; set; }
        public List<dynamic> Items { get; set; } = new();

        public async Task OnGetAsync()
        {
            var all = await _evSvc.GetAll();
            var q = all.Where(v => v.IsActive);
            if (CompanyId.HasValue) q = q.Where(v => v.CarCompanyId == CompanyId.Value);
            if (CategoryId.HasValue) q = q.Where(v => v.CategoryId == CategoryId.Value);
            if (!string.IsNullOrWhiteSpace(Q))
            {
                var k = Q.Trim().ToLowerInvariant();
                q = q.Where(v => (v.Model?.ToLower().Contains(k) ?? false)
                              || (v.Version?.ToLower().Contains(k) ?? false)
                              || (v.Color?.ToLower().Contains(k) ?? false));
            }

            Items = q.Select(v => new { v.Id, v.Model, v.Version, v.Price, v.ImageUrl, v.Color })
                     .Cast<dynamic>().ToList();

            CartChannel.EnsureChannel(HttpContext.Session);
        }

        public async Task<IActionResult> OnPostAddJson([FromBody] AddReq req)
        {
            if (req == null || req.Id <= 0) return new JsonResult(new { ok = false });

            var ev = await _evSvc.GetById(req.Id);
            if (ev == null || !ev.IsActive) return new JsonResult(new { ok = false });

            var cart = HttpContext.Session.GetCart();
            var it = cart.Items.FirstOrDefault(x => x.ElectricVehicleId == req.Id);
            if (it == null)
            {
                cart.Items.Add(new CartItem
                {
                    ElectricVehicleId = ev.Id,
                    Name = string.IsNullOrWhiteSpace(ev.Version) ? ev.Model : $"{ev.Model} {ev.Version}",
                    UnitPrice = ev.Price,
                    Quantity = Math.Max(1, req.Qty),
                    ImageUrl = ev.ImageUrl,
                    Color = ev.Color
                });
            }
            else
            {
                it.Quantity += Math.Max(1, req.Qty);
            }

            HttpContext.Session.SaveCart(cart);
            var count = cart.Items.Sum(x => x.Quantity);

            var channel = CartChannel.EnsureChannel(HttpContext.Session);
            await _hub.Clients.Group(channel).SendAsync("CartUpdated", count);

            return new JsonResult(new { ok = true, count });
        }

        public class AddReq { public int Id { get; set; } public int Qty { get; set; } }
    }
}
