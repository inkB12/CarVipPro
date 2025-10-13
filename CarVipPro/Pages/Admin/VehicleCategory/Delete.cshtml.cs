using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.VehicleCategory
{
    public class DeleteModel : PageModel
    {
        private readonly IVehicleCategoryService _service;

        public DeleteModel(IVehicleCategoryService service)
        {
            _service = service;
        }

        [BindProperty]
        public VehicleCategoryDTO Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _service.GetById(id);
            if (Category == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _service.Delete(Category.Id);
            return RedirectToPage("Index");
        }
    }

}
