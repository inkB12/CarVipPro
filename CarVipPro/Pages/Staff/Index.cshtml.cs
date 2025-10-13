using CarVipPro.APrenstationLayer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Staff
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32(SessionKeys.UserId);
            var role = HttpContext.Session.GetString(SessionKeys.Role);

            if (userId == null)
                return RedirectToPage("/Auth/Login", new { ReturnUrl = "/Staff/Index" });

            if (!string.Equals(role, "Staff", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(role, "Manager", StringComparison.OrdinalIgnoreCase))
                return StatusCode(403);

            return Page();
        }
    }
}
