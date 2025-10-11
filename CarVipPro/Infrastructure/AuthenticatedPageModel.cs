using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarVipPro.APrenstationLayer.Infrastructure
{
    public abstract class AuthenticatedPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
    {
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (!HttpContext.Session.GetInt32(SessionKeys.UserId).HasValue)
            {
                var returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                context.Result = new RedirectToPageResult("/Auth/Login", new { ReturnUrl = returnUrl });
                return;
            }
            base.OnPageHandlerExecuting(context);
        }
    }
}
