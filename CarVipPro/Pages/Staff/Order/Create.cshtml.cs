using System.ComponentModel.DataAnnotations;
using CarVipPro.APrenstationLayer.Infrastructure;
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarVipPro.APrenstationLayer.Pages.Staff.Order
{
    public class CreateModel : PageModel
    {
        private readonly IOrderService _orderService;
        public CreateModel(IOrderService orderService, IMomoService momoService)
        {
            _orderService = orderService;
        }

        [BindProperty] public InputModel Input { get; set; } = new();
        public string? Error { get; set; }
        public int OrderId { get; set; }
        public decimal OrderTotal { get; set; }

        public class InputModel
        {
            [Required] public int CustomerId { get; set; }
            [Required] public string PaymentMethod { get; set; } = "CASH";
            [Required] public int ElectricVehicleId { get; set; }
            [Range(1, int.MaxValue)] public int Quantity { get; set; } = 1;
            public decimal? TotalPrice { get; set; }
        }

        public IActionResult OnGet()
        {
            // bắt buộc đăng nhập (Session)
            var staffId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            var role = HttpContext.Session.GetString(SessionKeys.Role);
            if (staffId is null) return RedirectToPage("/Auth/Login", new { ReturnUrl = "/Orders/Create" });
            if (!string.Equals(role, "Staff", StringComparison.OrdinalIgnoreCase)
             && !string.Equals(role, "Manager", StringComparison.OrdinalIgnoreCase))
                return StatusCode(403);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var staffId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            var role = HttpContext.Session.GetString(SessionKeys.Role);
            if (staffId is null) return RedirectToPage("/Auth/Login", new { ReturnUrl = "/Orders/Create" });
            if (!string.Equals(role, "Staff", StringComparison.OrdinalIgnoreCase)
             && !string.Equals(role, "Manager", StringComparison.OrdinalIgnoreCase))
                return StatusCode(403);

            if (!ModelState.IsValid) return Page();

            var items = new[]
            {
                new OrderItemDto
                {
                    ElectricVehicleId = Input.ElectricVehicleId,
                    Quantity = Input.Quantity,
                    TotalPrice = Input.TotalPrice
                }
            };

            var (ok, msg, order) = await _orderService.CreatePaidOrderAsync(
                customerId: Input.CustomerId,
                staffAccountId: staffId.Value,
                paymentMethod: Input.PaymentMethod,
                items: items
            );

            if (!ok || order is null)
            {
                Error = msg;
                return Page();
            }

            // reset form
            ModelState.Clear();
            Input = new();
            return Page();
        }
    }
}
