

namespace CarVipPro.BLL.Dtos
{
    public class DriveScheduleViewDto
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = null!;
    }
}
