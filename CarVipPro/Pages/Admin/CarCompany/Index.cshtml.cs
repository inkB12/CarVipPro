using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.CarCompany
{
    public class IndexModel : PageModel
    {
        private readonly ICarCompanyService _service;

        public IndexModel(ICarCompanyService service)
        {
            _service = service;
        }

        public IEnumerable<CarCompanyDTO> CarCompanies { get; set; }

        public async Task OnGetAsync()
        {
            CarCompanies = await _service.GetAll();
        }
    }
}
