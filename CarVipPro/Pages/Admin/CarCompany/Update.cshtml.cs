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
            if (Company == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _service.Update(Company);
            return RedirectToPage("Index");
        }
    }
}
