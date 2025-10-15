using CarVipPro.APrenstationLayer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            var role = HttpContext.Session.GetString(SessionKeys.Role);

            if (userId == null)
            {
                // Chưa đăng nhập vào Login
                return RedirectToPage("/Auth/Login");
            }

            // Đã đăng nhập điều hướng theo role
            if (string.Equals(role, "Staff", StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("/Staff/Index");

            // ví dụ cho Manager (nếu có)
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("/Admin/Index");

            // fallback
            return RedirectToPage("/Auth/Login");
        }
    }
}
