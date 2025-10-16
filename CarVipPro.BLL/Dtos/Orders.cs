namespace CarVipPro.BLL.Dtos
{
    public class OrderListItemDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    
    public class OrderDetailItemDto
    {
        public string VehicleModel { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UnitPriceEstimated { get; set; }
    }
    
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public DateTime DateTime { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<OrderDetailItemDto> Details { get; set; } = new();
    }
}
