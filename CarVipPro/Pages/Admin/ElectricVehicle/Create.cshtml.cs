using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarVipPro.APrenstationLayer.Pages.Admin.ElectricVehicle
    
{
    public class CreateModel : PageModel
    {
        private readonly IElectricVehicleService _vehicleService;
        private readonly ICarCompanyService _carCompanyService;
        private readonly IVehicleCategoryService _categoryService;

        public CreateModel(
            IElectricVehicleService vehicleService,
            ICarCompanyService carCompanyService,
            IVehicleCategoryService categoryService)
        {
            _vehicleService = vehicleService;
            _carCompanyService = carCompanyService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public ElectricVehicleDTO Vehicle { get; set; }

        public List<SelectListItem> CarCompanies { get; set; }
        public List<SelectListItem> Categories { get; set; }

        public async Task OnGetAsync()
        {
            var companies = await _carCompanyService.GetAll();
            var categories = await _categoryService.GetAll();

            CarCompanies = companies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CatalogName
            }).ToList();

            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // reload dropdowns when validation fails
                return Page();
            }

            await _vehicleService.Add(Vehicle);
            return RedirectToPage("Index");
        }
    }
}
