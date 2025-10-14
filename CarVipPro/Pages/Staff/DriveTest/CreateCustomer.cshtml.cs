using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Staff.DriveTest
{
    public class CreateCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CreateCustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public CustomerDto NewCustomer { get; set; } = new();

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Vui lòng nhập đầy đủ thông tin bắt buộc.";
                return Page();
            }

            // ➕ Thêm mới khách hàng
            var created = await _customerService.CreateAsync(NewCustomer);

            if (created == null)
            {
                Message = "Không thể tạo khách hàng. Vui lòng thử lại.";
                return Page();
            }

            // ✅ Redirect sang trang tạo lịch lái thử
            return RedirectToPage("/Staff/DriveTest/Create", new { customerId = created.Id });
        }
    }
}
