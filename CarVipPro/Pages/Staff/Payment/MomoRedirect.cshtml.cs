using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Staff.Payment
{
    public class MomoRedirectModel : PageModel
    {
        private readonly IOrderService _orderService;

        public MomoRedirectModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public string PaymentMessage { get; set; }
        [BindProperty]
        public int OrderId { get; set; }
        [BindProperty]
        public int ResultCode { get; set; }


        public async Task<IActionResult> OnGetAsync(int resultCode, string orderId)
        {
            string status = "CANCELLED";
            string subStringId = orderId[35..];
            ResultCode = resultCode;

            if (resultCode == 0)
            {
                // Succesful Payment
                PaymentMessage = "Đơn hàng thanh toán thành công";
                status = "COMPLETED";
            }
            else
            {
                // Fail Payment
                PaymentMessage = "Đơn hàng thanh toán thất bại";
            }

            if (int.TryParse(subStringId, out int id))
            {
                await _orderService.UpdateOrderStatusAsync(status, id);
                OrderId = id;
            }
            else
            {
                PaymentMessage = "Đơn hàng thanh toán thất bại do mã đơn lỗi";
            }

            return Page();
        }
    }
}
