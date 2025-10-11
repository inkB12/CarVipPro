using CarVipPro.DAL.Entities;

namespace CarVipPro.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAsync(bool onlyActive = true);
        Task<Account?> GetByIdAsync(int id);

        Task<(bool ok, string message, Account? data)> RegisterAsync(
            string email, string password, string fullName, string? phone, string role = "Staff");

        Task<(bool ok, string message, Account? data)> LoginAsync(string email, string password);

        Task<(bool ok, string message, Account? data)> CreateAsync(Account account);
        Task<(bool ok, string message, Account? data)> UpdateAsync(Account account);

        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
