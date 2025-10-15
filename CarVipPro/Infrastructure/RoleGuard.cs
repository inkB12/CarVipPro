using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Infrastructure
{
    public static class RoleGuard
    {
        public static string? CurrentRole(HttpContext http) =>
            http?.Session?.GetString(SessionKeys.Role);

        public static bool HasRole(HttpContext http, string role)
        {
            var r = CurrentRole(http);
            return !string.IsNullOrWhiteSpace(r)
                   && string.Equals(r, role, StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasAnyRole(HttpContext http, params string[] roles)
        {
            var r = CurrentRole(http);
            if (string.IsNullOrWhiteSpace(r) || roles == null || roles.Length == 0) return false;
            return roles.Any(x => string.Equals(r, x, StringComparison.OrdinalIgnoreCase));
        }

        public static IActionResult? BlockIfNotRole(PageModel page, string requiredRole, string? redirectPage = null)
        {
            if (page?.HttpContext is null) return new StatusCodeResult(500);

            if (HasRole(page.HttpContext, requiredRole)) return null;

            if (!string.IsNullOrWhiteSpace(redirectPage))
                return page.RedirectToPage(redirectPage);

            page.Response.StatusCode = StatusCodes.Status403Forbidden;
            return new EmptyResult();
        }

        public static IActionResult? BlockIfNotAnyRole(PageModel page, params string[] allowedRoles)
        {
            if (page?.HttpContext is null) return new StatusCodeResult(500);

            if (HasAnyRole(page.HttpContext, allowedRoles)) return null;

            page.Response.StatusCode = StatusCodes.Status403Forbidden;
            return new EmptyResult();
        }
    }
}
