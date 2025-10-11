using CarVipPro.DAL.Entities;
using System.Linq.Expressions;


namespace CarVipPro.DAL.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(int id);
        Task<Account?> GetByEmailAsync(string email);
        Task<List<Account>> GetAllAsync(Expression<Func<Account, bool>>? predicate = null);

        Task<Account> CreateAsync(Account account);
        Task<Account> UpdateAsync(Account account);        
        Task<bool> DeleteAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
