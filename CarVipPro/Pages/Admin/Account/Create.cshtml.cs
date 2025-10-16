using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IAccountService _service;
        public CreateModel(IAccountService service) { _service = service; }

        [BindProperty] public AccountDTO Input { get; set; } = new() { Role = "Staff", IsActive = true };
        [BindProperty] public string Password { get; set; } = "";

        public string? Error { get; set; }
        public string? Info { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Input.Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Input.FullName))
            {
                Error = "Email, Họ tên, Mật khẩu là bắt buộc.";
                return Page();
            }

            var (ok, msg, data) = await _service.CreateAsync(Input, Password);
            if (!ok || data == null)
            {
                Error = msg;
                return Page();
            }

            TempData["Info"] = "Tạo tài khoản thành công.";
            return RedirectToPage("Index");
        }
    }
}
