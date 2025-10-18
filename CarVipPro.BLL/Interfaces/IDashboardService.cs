

using CarVipPro.BLL.Dtos;

namespace CarVipPro.BLL.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(int? year = null, int? month = null);
        Task<List<SalesByCompanyDto>> GetTopCompaniesAsync(int year);
        Task<List<SalesByModelDto>> GetTopModelsAsync(int year);
        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year);
        Task<List<YearlyRevenueDto>> GetYearlyRevenueAsync(); // ✅ thêm dòng này
    }
}
