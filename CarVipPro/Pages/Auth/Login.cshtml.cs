using CarVipPro.APrenstationLayer.Infrastructure;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarVipPro.APrenstationLayer.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _svc;
        public LoginModel(IAccountService svc) => _svc = svc;

        [BindProperty] public InputModel Input { get; set; } = new();
        [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }
        public string? Error { get; set; }

        public class InputModel
        {
            [Required, EmailAddress] public string Email { get; set; } = string.Empty;
            [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var (ok, msg, acc) = await _svc.LoginAsync(Input.Email, Input.Password);
            if (!ok || acc == null)
            {
                Error = msg ?? "Đăng nhập thất bại.";
                return Page();
            }

            HttpContext.Session.SignIn(acc.Id, acc.Email, acc.FullName ?? acc.Email, acc.Role ?? "Staff");

            if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return LocalRedirect(ReturnUrl);

            return RedirectToPage("/Index");
        }
    }
}
