using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.CarCompany
{
    public class DeleteModel : PageModel
    {
        private readonly ICarCompanyService _service;

        public DeleteModel(ICarCompanyService service)
        {
            _service = service;
        }

        [BindProperty]
        public CarCompanyDTO Company { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var company = await _service.GetById(id);
            if (company == null)
                return NotFound();

            Company = company;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Company == null)
                return NotFound();

            await _service.Delete(Company.Id);
            return RedirectToPage("Index");
        }
    }
}
