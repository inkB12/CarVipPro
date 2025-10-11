using CarVipPro.APrenstationLayer.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public void OnGet() { }

        [ValidateAntiForgeryToken]
        public IActionResult OnPost()
        {
            HttpContext.Session.SignOut();
            foreach (var k in Request.Cookies.Keys) Response.Cookies.Delete(k); // optional

            return RedirectToPage("/Auth/Login");
        }
    }
}
