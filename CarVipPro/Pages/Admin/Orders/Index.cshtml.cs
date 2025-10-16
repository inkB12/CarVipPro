using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        public IndexModel(IOrderService orderService) { _orderService = orderService; }


        public string? Error { get; set; }
        [BindProperty(SupportsGet = true)] public string? Q { get; set; }
        [BindProperty(SupportsGet = true)] public string? Status { get; set; }
        public List<OrderListItemDto> Items { get; set; } = new();


        public async Task OnGet()
        {
            try
            {
                Items = await _orderService.GetOrdersAsync(Q, Status);
            }
            catch (Exception ex)
            {
                Error = $"Lỗi tải đơn hàng: {ex.Message}";
            }
        }
    }
}
