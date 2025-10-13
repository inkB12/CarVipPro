using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace CarVipPro.APrenstationLayer.Pages.Admin.ElectricVehicle
{
    public class IndexModel : PageModel
    {
        private readonly IElectricVehicleService _service;

        public IndexModel(IElectricVehicleService service)
        {
            _service = service;
        }

        public IEnumerable<ElectricVehicleDTO> Vehicles { get; set; }

        public async Task OnGetAsync()
        {
            Vehicles = await _service.GetAll();
        }
    }
}
