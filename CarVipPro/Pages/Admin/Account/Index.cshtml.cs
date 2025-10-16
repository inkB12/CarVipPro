using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _service;
        public IndexModel(IAccountService service) { _service = service; }

        [BindProperty(SupportsGet = true)] public string? Q { get; set; }
        [BindProperty(SupportsGet = true)] public string? Role { get; set; }
        [BindProperty(SupportsGet = true)] public bool OnlyActive { get; set; } = true;

        public List<AccountDTO> Items { get; set; } = new();
        public string? Error { get; set; }

        public async Task OnGet()
        {
            try
            {
                var list = await _service.GetAllAsync(OnlyActive);
                if (!string.IsNullOrWhiteSpace(Role))
                    list = list.Where(x => string.Equals(x.Role, Role, StringComparison.OrdinalIgnoreCase)).ToList();
                if (!string.IsNullOrWhiteSpace(Q))
                {
                    var q = Q.Trim();
                    list = list.Where(x =>
                        (x.Email?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (x.FullName?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (x.Phone?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
                    ).ToList();
                }
                Items = list.OrderBy(x => x.Role).ThenBy(x => x.FullName).ToList();
            }
            catch (Exception ex)
            {
                Error = $"Lỗi tải danh sách: {ex.Message}";
            }
        }
    }
}
