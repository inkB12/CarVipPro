namespace CarVipPro.BLL.Dtos
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string FullName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
