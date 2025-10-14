

namespace CarVipPro.BLL.Dtos
{
    public class DriveScheduleSummaryDto
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = null!;
    }
}
