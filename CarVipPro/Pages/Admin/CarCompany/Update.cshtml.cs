using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.CarCompany
{
    public class EditModel : PageModel
    {
        private readonly ICarCompanyService _service;

        [BindProperty]
        public CarCompanyDTO Company { get; set; }

        public EditModel(ICarCompanyService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Company = await _service.GetById(id);
            if (Company == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hãng xe.";
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // 🔍 Kiểm tra trùng tên (loại trừ chính bản ghi đang sửa)
            var allCompanies = await _service.GetAll();
            bool isDuplicate = allCompanies
                .Any(c => c.CatalogName.Trim().ToLower() == Company.CatalogName.Trim().ToLower()
                       && c.Id != Company.Id);

            if (isDuplicate)
            {
                ModelState.AddModelError("Company.CatalogName", "⚠️ This name of Car Company is exist. Please input another name");
                return Page();
            }

            try
            {
                await _service.Update(Company);
                TempData["SuccessMessage"] = "✅ Update Successfull";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"❌ Lỗi: {ex.Message}");
                return Page();
            }
        }
    }
}
