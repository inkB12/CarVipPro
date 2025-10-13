using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.CarCompany
{
    public class CreateModel : PageModel
    {
        private readonly ICarCompanyService _service;

        [BindProperty]
        public CarCompanyDTO Company { get; set; }

        public CreateModel(ICarCompanyService service)
        {
            _service = service;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _service.Add(Company);
            return RedirectToPage("Index");
        }
    }
}
