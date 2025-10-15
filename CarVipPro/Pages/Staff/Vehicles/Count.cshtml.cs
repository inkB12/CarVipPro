using CarVipPro.APrenstationLayer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Staff.Vehicles
{
    public class CountModel : PageModel
    {
        public IActionResult OnGet()
        {
            var cart = HttpContext.Session.GetCart();
            var count = cart.Items.Sum(x => x.Quantity);
            return new JsonResult(new { count });
        }
    }
}
