namespace CarVipPro.BLL.Dtos
{
    public class OrderItemDto
    {
        public int ElectricVehicleId { get; set; }
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
