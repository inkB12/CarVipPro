

namespace CarVipPro.BLL.Dtos
{
    public class DriveScheduleCreateDto
    {
        public int ElectricVehicleId { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }   // Staff tạo lịch
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string? Status { get; set; }
        public int? Id { get; set; }
    }
}
