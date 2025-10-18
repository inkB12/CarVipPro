using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public DashboardSummaryDto Summary { get; set; } = new();
        public List<YearlyRevenueDto> YearlyRevenues { get; set; } = new(); // ✅ thêm
        public int SelectedYear { get; set; }
        public int? SelectedMonth { get; set; }

        public IndexModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task OnGetAsync(int? year, int? month)
        {
            SelectedYear = year ?? DateTime.Now.Year;
            SelectedMonth = month;

            Summary = await _dashboardService.GetSummaryAsync(SelectedYear, SelectedMonth);
            YearlyRevenues = await _dashboardService.GetYearlyRevenueAsync(); // ✅ thêm
        }
    }
}
