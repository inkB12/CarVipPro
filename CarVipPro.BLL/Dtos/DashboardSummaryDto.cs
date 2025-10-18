
namespace CarVipPro.BLL.Dtos
{
    public class DashboardSummaryDto
    {
        public int TotalCompanies { get; set; }
        public int TotalVehicles { get; set; }
        public int TotalCategories { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }

        public List<MonthlyRevenueDto> MonthlyRevenues { get; set; } = new();
        public List<SalesByCompanyDto> TopCompanies { get; set; } = new();
        public List<SalesByModelDto> TopModels { get; set; } = new();
    }
}
