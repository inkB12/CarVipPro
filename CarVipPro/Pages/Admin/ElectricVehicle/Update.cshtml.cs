using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarVipPro.APrenstationLayer.Pages.Admin.ElectricVehicle
{
    public class EditModel : PageModel
    {
        private readonly IElectricVehicleService _vehicleService;
        private readonly ICarCompanyService _carCompanyService;
        private readonly IVehicleCategoryService _categoryService;

        public EditModel(
            IElectricVehicleService vehicleService,
            ICarCompanyService carCompanyService,
            IVehicleCategoryService categoryService)
        {
            _vehicleService = vehicleService;
            _carCompanyService = carCompanyService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public ElectricVehicleDTO Vehicle { get; set; } = new();

        public List<SelectListItem> CarCompanies { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();

        // ✅ Load form Edit
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vehicle = await _vehicleService.GetById(id);
            if (Vehicle == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy xe điện cần chỉnh sửa.";
                return RedirectToPage("Index");
            }

            await LoadDropdownsAsync();
            return Page();
        }

        // ✅ Submit Update
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            try
            {
                await _vehicleService.Update(Vehicle);
                TempData["SuccessMessage"] = "Cập nhật xe điện thành công!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadDropdownsAsync();
                return Page();
            }
        }

        // ✅ Hàm nạp Dropdown danh sách Hãng & Loại xe
        private async Task LoadDropdownsAsync()
        {
            var companies = await _carCompanyService.GetAll();
            var categories = await _categoryService.GetAll();

            CarCompanies = companies.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CatalogName,
                Selected = Vehicle != null && c.Id == Vehicle.CarCompanyId
            }).ToList();

            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName,
                Selected = Vehicle != null && c.Id == Vehicle.CategoryId
            }).ToList();
        }
    }
}
