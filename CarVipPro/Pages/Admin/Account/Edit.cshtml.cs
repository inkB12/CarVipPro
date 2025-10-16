using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.Accounts
{
    public class EditModel : PageModel
    {
        private readonly IAccountService _service;
        public EditModel(IAccountService service) { _service = service; }

        [BindProperty] public AccountDTO? Input { get; set; }
        [BindProperty] public string? NewPassword { get; set; }

        public string? Error { get; set; }
        public string? Info { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Input = await _service.GetByIdAsync(id);
            if (Input == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input == null) return BadRequest();
            var (ok, msg, data) = await _service.UpdateAsync(Input, string.IsNullOrWhiteSpace(NewPassword) ? null : NewPassword);
            if (!ok || data == null)
            {
                Error = msg;
                return Page();
            }
            TempData["Info"] = "Cập nhật thành công.";
            return RedirectToPage("Index");
        }
    }
}
