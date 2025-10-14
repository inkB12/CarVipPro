using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Staff.DriveTest
{
    public class SearchCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public SearchCustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        public List<CustomerDto> SearchResults { get; set; } = new();

        [BindProperty]
        public CustomerDto NewCustomer { get; set; } = new();

        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                // 1️⃣ Tìm danh sách khách hàng cơ bản
                var basicList = await _customerService.SearchAsync(Keyword);

                if (basicList.Count == 0)
                {
                    Message = "Không tìm thấy khách hàng nào phù hợp.";
                    return;
                }

                // 2️⃣ Duyệt tuần tự để nạp thêm lịch lái thử (tránh lỗi DbContext)
                SearchResults = new List<CustomerDto>();
                foreach (var c in basicList)
                {
                    var detail = await _customerService.GetByIdAsync(c.Id);
                    if (detail != null)
                        SearchResults.Add(detail);
                }
            }
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Vui lòng nhập đầy đủ thông tin.";
                return Page();
            }

            var created = await _customerService.CreateAsync(NewCustomer);

            // ✅ Redirect đúng tới trang Create (đặt lịch lái thử)
            return RedirectToPage("/Staff/DriveTest/Create", new { customerId = created.Id });
        }

        public IActionResult OnPostSelect(int customerId)
        {
            // ✅ Redirect đúng tới trang Create (đặt lịch lái thử)
            return RedirectToPage("/Staff/DriveTest/Create", new { customerId });
        }
    }
}
