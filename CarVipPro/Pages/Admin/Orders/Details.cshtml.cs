using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderService _orderService;
        public DetailsModel(IOrderService orderService) { _orderService = orderService; }


        public string? Error { get; set; }
        public OrderDto? Order { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Order = await _orderService.GetOrderAsync(id);
                if (Order == null) return NotFound();
                return Page();
            }
            catch (Exception ex)
            {
                Error = $"Lỗi tải chi tiết: {ex.Message}";
                return Page();
            }
        }
    }
}
