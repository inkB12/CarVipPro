using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Staff.Payment
{
    public class VNPayRedirectModel : PageModel
    {
        private readonly IOrderService _orderService;

        public VNPayRedirectModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public string PaymentMessage { get; set; }
        [BindProperty]
        public int OrderId { get; set; }
        [BindProperty]
        public string ResultCode { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var response = HttpContext.Request.Query;
            string status = "CANCELLED";

            // Validation
            if (response.Count == 0)
            {
                PaymentMessage = "Không tìm thấy dữ liệu phản hồi từ VNPay.";
                return Page();
            }

            ResultCode = response["vnp_ResponseCode"];

            string orderId = response["vnp_OrderInfo"];
            string subStringId = orderId[35..];

            if (ResultCode == "00")
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
