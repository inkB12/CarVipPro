

using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Interfaces;

namespace CarVipPro.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IElectricVehicleRepository _vehicleRepo;
        private readonly ICarCompanyRepository _companyRepo;
        private readonly IVehicleCategoryRepository _categoryRepo;
        private readonly ICustomerRepository _customerRepo;

        public DashboardService(
            IOrderRepository orderRepo,
            IElectricVehicleRepository vehicleRepo,
            ICarCompanyRepository companyRepo,
            IVehicleCategoryRepository categoryRepo,
            ICustomerRepository customerRepo)
        {
            _orderRepo = orderRepo;
            _vehicleRepo = vehicleRepo;
            _companyRepo = companyRepo;
            _categoryRepo = categoryRepo;
            _customerRepo = customerRepo;
        }

        // 🔹 Lấy thống kê tổng hợp cho Dashboard
        public async Task<DashboardSummaryDto> GetSummaryAsync(int? year = null, int? month = null)
        {
            var vehicles = await _vehicleRepo.GetAllAsync();
            var companies = await _companyRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            var customers = await _customerRepo.SearchAsync(""); // đếm khách hàng hiện có
            var orders = await _orderRepo.GetAllWithDetailsAsync(); // ✅ dùng hàm mới

            // Lọc theo năm & tháng
            if (year.HasValue)
                orders = orders.Where(o => o.DateTime.Year == year.Value).ToList();
            if (month.HasValue)
                orders = orders.Where(o => o.DateTime.Month == month.Value).ToList();

            // Chỉ tính đơn hàng đã hoàn tất
            var completedOrders = orders.Where(o => o.Status == "COMPLETED").ToList();

            // Tổng doanh thu & đơn hàng
            decimal totalRevenue = completedOrders.Sum(o => o.Total);
            int totalOrders = completedOrders.Count;

            // 🔸 Doanh thu theo tháng (để vẽ biểu đồ cột)
            var monthlyRevenue = completedOrders
                .GroupBy(o => o.DateTime.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Month = g.Key,
                    Revenue = g.Sum(o => o.Total)
                })
                .OrderBy(x => x.Month)
                .ToList();

            // 🔸 Top hãng xe bán chạy nhất
            var topCompanies = completedOrders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(d => d.ElectricVehicle.CarCompany.CatalogName)
                .Select(g => new SalesByCompanyDto
                {
                    CompanyName = g.Key,
                    VehiclesSold = g.Sum(d => d.Quantity)
                })
                .OrderByDescending(x => x.VehiclesSold)
                .Take(5)
                .ToList();

            // 🔸 Top model xe bán chạy nhất
            var topModels = completedOrders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(d => d.ElectricVehicle.Model)
                .Select(g => new SalesByModelDto
                {
                    ModelName = g.Key,
                    QuantitySold = g.Sum(d => d.Quantity),
                    TotalRevenue = g.Sum(d => d.TotalPrice)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToList();

        
            // ✅ Trả dữ liệu tổng hợp
            return new DashboardSummaryDto
            {
                TotalCompanies = companies.Count(),
                TotalVehicles = vehicles.Count(),
                TotalCategories = categories.Count(),
                TotalCustomers = customers.Count,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                MonthlyRevenues = monthlyRevenue,
                TopCompanies = topCompanies,
                TopModels = topModels,
            };
        }

        // 🔹 Doanh thu từng tháng trong năm
        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
        {
            var orders = await _orderRepo.GetAllWithDetailsAsync();
            var completedOrders = orders.Where(o => o.Status == "COMPLETED" && o.DateTime.Year == year);

            return completedOrders
                .GroupBy(o => o.DateTime.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Month = g.Key,
                    Revenue = g.Sum(o => o.Total)
                })
                .OrderBy(x => x.Month)
                .ToList();
        }

        // 🔹 Doanh thu theo từng năm (cho biểu đồ tổng)
        public async Task<List<YearlyRevenueDto>> GetYearlyRevenueAsync()
        {
            var orders = await _orderRepo.GetAllWithDetailsAsync();
            var completedOrders = orders.Where(o => o.Status == "COMPLETED");

            return completedOrders
                .GroupBy(o => o.DateTime.Year)
                .Select(g => new YearlyRevenueDto
                {
                    Year = g.Key,
                    Revenue = g.Sum(o => o.Total)
                })
                .OrderBy(x => x.Year)
                .ToList();
        }

        // 🔹 Top 5 hãng xe bán chạy theo năm
        public async Task<List<SalesByCompanyDto>> GetTopCompaniesAsync(int year)
        {
            var orders = await _orderRepo.GetAllWithDetailsAsync();
            var completedOrders = orders.Where(o => o.Status == "COMPLETED" && o.DateTime.Year == year);

            return completedOrders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(d => d.ElectricVehicle.CarCompany.CatalogName)
                .Select(g => new SalesByCompanyDto
                {
                    CompanyName = g.Key,
                    VehiclesSold = g.Sum(d => d.Quantity)
                })
                .OrderByDescending(x => x.VehiclesSold)
                .Take(5)
                .ToList();
        }

        // 🔹 Top 5 model xe bán chạy theo năm
        public async Task<List<SalesByModelDto>> GetTopModelsAsync(int year)
        {
            var orders = await _orderRepo.GetAllWithDetailsAsync();
            var completedOrders = orders.Where(o => o.Status == "COMPLETED" && o.DateTime.Year == year);

            return completedOrders
                .SelectMany(o => o.OrderDetails)
                .GroupBy(d => d.ElectricVehicle.Model)
                .Select(g => new SalesByModelDto
                {
                    ModelName = g.Key,
                    QuantitySold = g.Sum(d => d.Quantity),
                    TotalRevenue = g.Sum(d => d.TotalPrice)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToList();
        }
    }
}
