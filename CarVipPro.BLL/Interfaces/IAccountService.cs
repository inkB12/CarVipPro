using CarVipPro.BLL.Dtos;
using CarVipPro.DAL.Entities;

namespace CarVipPro.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountDTO>> GetAllAsync(bool onlyActive = true);
        Task<AccountDTO?> GetByIdAsync(int id);
        Task<(bool ok, string message, AccountDTO? data)> RegisterAsync(
            string email, string password, string fullName, string? phone, string role = "Staff");
        Task<AccountDTO?> LoginAsync(string email, string password);
        Task<(bool ok, string message, AccountDTO? data)> CreateAsync(AccountDTO dto, string password);
        Task<(bool ok, string message, AccountDTO? data)> UpdateAsync(AccountDTO dto, string? newPassword = null);
        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
