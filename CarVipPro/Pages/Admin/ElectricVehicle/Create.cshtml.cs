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
        public ElectricVehicleDTO Vehicle { get; set; } = new();

        public List<SelectListItem> CarCompanies { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();

        // ✅ Khi mở trang Create
        public async Task OnGetAsync()
        {
            await LoadDropdownsAsync();
        }

        // ✅ Nạp dữ liệu dropdown Hãng xe và Loại xe
        private async Task LoadDropdownsAsync()
        {
            var companies = await _carCompanyService.GetAll();
            var categories = await _categoryService.GetAll();

            CarCompanies = companies
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CatalogName
                }).ToList();

            Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CategoryName
                }).ToList();
        }

        // ✅ Xử lý khi nhấn Lưu
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(); // ← đổi đúng tên
                return Page();
            }

            try
            {
                await _vehicleService.Add(Vehicle);
                TempData["SuccessMessage"] = "Thêm xe điện thành công!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadDropdownsAsync(); // ← đổi đúng tên
                return Page();
            }
        }
    }
}
