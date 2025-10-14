using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.VehicleCategory
{
    public class EditModel : PageModel
    {
        private readonly IVehicleCategoryService _service;

        [BindProperty]
        public VehicleCategoryDTO Category { get; set; }

        public EditModel(IVehicleCategoryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _service.GetById(id);
            if (Category == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.Update(Category);
                TempData["SuccessMessage"] = "Cập nhật loại xe thành công!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
