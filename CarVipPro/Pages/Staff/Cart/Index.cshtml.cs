using CarVipPro.APrenstationLayer.Infrastructure;
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace CarVipPro.APrenstationLayer.Pages.Staff.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IMomoService _momoService;
        private readonly ICustomerService _customers; // <-- thêm service khách hàng

        public IndexModel(IOrderService orderService, IMomoService momoService, ICustomerService customers)
        {
            _orderService = orderService;
            _momoService = momoService;
            _customers = customers;
        }

        public CartModel Cart { get; set; } = new();
        [BindProperty(SupportsGet = true)] public int CustomerId { get; set; }
        [BindProperty(SupportsGet = true)] public string PaymentMethod { get; set; } = "CASH";
        public string? Error { get; set; }
        public string? Info { get; set; }

        public void OnGet()
        {
            Cart = HttpContext.Session.GetCart();
        }

        // ---------- AJAX: search customer by name/email/phone ----------
        // GET /Staff/Cart/Index?handler=CustomerSearch&q=...
        public async Task<IActionResult> OnGetCustomerSearchAsync(string? q)
        {
            q = (q ?? "").Trim();
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
                return new JsonResult(Array.Empty<object>());

            // Yêu cầu ICustomerService có hàm SearchAsync(q)
            // Trả về list entity/DTO; ở đây map về dạng nhẹ cho UI
            var list = await _customers.SearchAsync(q);
            var result = list.Select(c => new
            {
                id = c.Id,
                fullName = c.FullName,
                email = c.Email,
                phone = c.Phone
            }).ToList();

            return new JsonResult(result);
        }
        // ---------------------------------------------------------------

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

            if (CustomerId <= 0)
            {
                Error = "Vui lòng chọn khách hàng từ ô tìm kiếm (tối thiểu 2 ký tự để tìm).";
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

            if (order.PaymentMethod.Equals("MOMO"))
            {
                // Create Payment Url
                var momoResponse = await _momoService.CreatePaymentAsync(order.Id, order.Total);

                if (momoResponse != null && !momoResponse.PayUrl.IsNullOrEmpty())
                {
                    return Redirect(momoResponse.PayUrl);
                }
            }

            // thanh toán thành công
            HttpContext.Session.ClearCart();
            Info = $"Thanh toán thành công. OrderId = {order.Id}, Tổng = {order.Total:n0}";
            Cart = new CartModel(); // clear
            return Page();
        }
    }
}
