using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.VehicleCategory { 
    public class CreateModel : PageModel
    {
        private readonly IVehicleCategoryService _service;

        [BindProperty]
        public VehicleCategoryDTO Category { get; set; }

        public CreateModel(IVehicleCategoryService service)
        {
            _service = service;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _service.Add(Category);
            return RedirectToPage("Index");
        }
    }
}
