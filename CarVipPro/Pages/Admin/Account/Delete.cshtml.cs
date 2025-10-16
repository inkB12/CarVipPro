using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly IAccountService _service;
        public DeleteModel(IAccountService service) { _service = service; }

        [BindProperty(SupportsGet = true)] public int Id { get; set; }
        public AccountDTO? Item { get; set; }
        public string? Error { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Item = await _service.GetByIdAsync(Id);
            if (Item == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var (ok, msg) = await _service.DeleteAsync(Id);
            if (!ok)
            {
                Error = msg;
                Item = await _service.GetByIdAsync(Id);
                return Page();
            }
            TempData["Info"] = "Đã xóa tài khoản.";
            return RedirectToPage("Index");
        }
    }
}
