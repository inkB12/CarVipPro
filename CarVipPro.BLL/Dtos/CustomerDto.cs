

namespace CarVipPro.BLL.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? IdentityCard { get; set; }
        public string? Address { get; set; }
        public string? ZipCode { get; set; }

        public List<DriveScheduleSummaryDto> DriveSchedules { get; set; } = new();
    }
}
