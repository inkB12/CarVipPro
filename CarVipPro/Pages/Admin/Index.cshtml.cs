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
        private readonly IOrderService _orderService;
        public int TotalCompanies { get; set; }
        public int TotalVehicles { get; set; }
        public int TotalCategories { get; set; }
        public int TotalOrders { get; set; }
        public IndexModel(
            ICarCompanyService carCompanyService,
            IElectricVehicleService electricVehicleService,
            IVehicleCategoryService vehicleCategoryService,
            IOrderService orderService)
           
        {
            _carCompanyService = carCompanyService;
            _electricVehicleService = electricVehicleService;
            _vehicleCategoryService = vehicleCategoryService;
            _orderService = orderService;
        }

        public async Task OnGetAsync()
        {
            var companies = await _carCompanyService.GetAll();
            var vehicles = await _electricVehicleService.GetAll();
            var categories = await _vehicleCategoryService.GetAll();
            var order = await _orderService.GetOrdersAsync(null, "COMPLETED");

            TotalCompanies = companies.Count();
            TotalVehicles = vehicles.Count();
            TotalCategories = categories.Count();
            TotalOrders = order.Count();
        }
    }
}
