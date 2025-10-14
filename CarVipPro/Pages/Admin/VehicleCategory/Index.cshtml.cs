using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.VehicleCategory
{
    public class IndexModel : PageModel
    {
        private readonly IVehicleCategoryService _service;

        public IndexModel(IVehicleCategoryService service)
        {
            _service = service;
        }

        public IEnumerable<VehicleCategoryDTO> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _service.GetAll();
        }
    }
}
