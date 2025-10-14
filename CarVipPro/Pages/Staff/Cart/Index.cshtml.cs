using CarVipPro.APrenstationLayer.Infrastructure;
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Staff.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        public IndexModel(IOrderService orderService) => _orderService = orderService;

        public CartModel Cart { get; set; } = new();
        [BindProperty(SupportsGet = true)] public int CustomerId { get; set; }
        [BindProperty(SupportsGet = true)] public string PaymentMethod { get; set; } = "CASH";
        public string? Error { get; set; }
        public string? Info { get; set; }

        public void OnGet()
        {
            Cart = HttpContext.Session.GetCart();
        }

        //public IActionResult OnPostUpdate([FromForm] List<CartItem> Items)
        //{
        //    var cart = HttpContext.Session.GetCart();

        //    foreach (var input in Items)
        //    {
        //        var it = cart.Items.FirstOrDefault(x => x.ElectricVehicleId == input.ElectricVehicleId);
        //        if (it != null)
        //        {
        //            it.Quantity = Math.Max(1, input.Quantity);
        //        }
        //    }

        //    HttpContext.Session.SaveCart(cart);
        //    Cart = cart;
        //    Info = "Đã cập nhật giỏ hàng.";
        //    return Page();
        //}

        //public IActionResult OnPostRemove(int id)
        //{
        //    var cart = HttpContext.Session.GetCart();
        //    cart.Items.RemoveAll(i => i.ElectricVehicleId == id);
        //    HttpContext.Session.SaveCart(cart);

        //    Cart = cart;
        //    Info = "Đã xoá sản phẩm khỏi giỏ.";
        //    return Page();
        //}
        
        public async Task<IActionResult> OnPostCheckout(int CustomerId, string PaymentMethod)
        {
            var staffId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            var role = HttpContext.Session.GetString(SessionKeys.Role);
            if (staffId is null) return RedirectToPage("/Auth/Login", new { ReturnUrl = "/Cart/Index" });
            if (!string.Equals(role, "Staff", StringComparison.OrdinalIgnoreCase)
             && !string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                Error = "Bạn không có quyền thanh toán.";
                Cart = HttpContext.Session.GetCart();
                return Page();
            }

            var cart = HttpContext.Session.GetCart();
            if (cart.Items.Count == 0)
            {
                Error = "Giỏ hàng trống.";
                Cart = cart;
                return Page();
            }

            var items = cart.Items.Select(i => new OrderItemDto
            {
                ElectricVehicleId = i.ElectricVehicleId,
                Quantity = i.Quantity,
                TotalPrice = i.UnitPrice * i.Quantity
            }).ToList();

            var (ok, msg, order) = await _orderService.CreatePaidOrderAsync(
                customerId: CustomerId,
                staffAccountId: staffId.Value,
                paymentMethod: PaymentMethod,
                items: items
            );

            if (!ok || order == null)
            {
                Error = msg;
                Cart = cart;
                return Page();
            }

            //thanh toán thành công
            HttpContext.Session.ClearCart();
            Info = $"Thanh toán thành công. OrderId = {order.Id}, Tổng = {order.Total:n0}";
            Cart = new CartModel();// clear
            return Page();
        }
    }
}
