

namespace CarVipPro.BLL.Dtos
{
    public class SalesByModelDto
    {
        public string ModelName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
