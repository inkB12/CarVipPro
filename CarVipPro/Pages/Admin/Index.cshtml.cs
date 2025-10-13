using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ICarCompanyService _carCompanyService;
        private readonly IElectricVehicleService _electricVehicleService;
        private readonly IVehicleCategoryService _vehicleCategoryService;

        public int TotalCompanies { get; set; }
        public int TotalVehicles { get; set; }
        public int TotalCategories { get; set; }

        public IndexModel(
            ICarCompanyService carCompanyService,
            IElectricVehicleService electricVehicleService,
            IVehicleCategoryService vehicleCategoryService)
        {
            _carCompanyService = carCompanyService;
            _electricVehicleService = electricVehicleService;
            _vehicleCategoryService = vehicleCategoryService;
        }

        public async Task OnGetAsync()
        {
            var companies = await _carCompanyService.GetAll();
            var vehicles = await _electricVehicleService.GetAll();
            var categories = await _vehicleCategoryService.GetAll();

            TotalCompanies = companies.Count();
            TotalVehicles = vehicles.Count();
            TotalCategories = categories.Count();
        }
    }
}
