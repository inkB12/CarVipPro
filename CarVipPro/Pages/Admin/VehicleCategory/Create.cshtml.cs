using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.Add(Category);
                TempData["SuccessMessage"] = "Thêm loại xe thành công!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // Nếu lỗi do trùng tên, hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
