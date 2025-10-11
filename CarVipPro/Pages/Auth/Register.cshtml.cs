using CarVipPro.APrenstationLayer.Infrastructure;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarVipPro.APrenstationLayer.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _svc;
        public RegisterModel(IAccountService svc) => _svc = svc;

        [BindProperty] public InputModel Input { get; set; } = new();
        [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }
        public string? Error { get; set; }

        public class InputModel
        {
            [Required, Display(Name = "Họ tên")] public string FullName { get; set; } = string.Empty;
            [Required, EmailAddress] public string Email { get; set; } = string.Empty;
            [Phone, Display(Name = "Số điện thoại")] public string? Phone { get; set; }
            [Required, DataType(DataType.Password), MinLength(6)] public string Password { get; set; } = string.Empty;
            [Required, DataType(DataType.Password), Display(Name = "Xác nhận mật khẩu"),
             Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var (ok, msg, acc) = await _svc.RegisterAsync(Input.Email, Input.Password, Input.FullName, Input.Phone, role: "Staff");
            if (!ok || acc == null)
            {
                Error = msg ?? "Đăng ký thất bại.";
                return Page();
            }

            // Auto-login sau đăng ký
            HttpContext.Session.SignIn(acc.Id, acc.Email, acc.FullName ?? acc.Email, acc.Role ?? "Staff");

            if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return LocalRedirect(ReturnUrl);

            return RedirectToPage("/Index");
        }
    }
}
