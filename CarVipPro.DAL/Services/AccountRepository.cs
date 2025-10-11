using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarVipPro.DAL.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CarVipProContext _db;

        public AccountRepository(CarVipProContext db)
        {
            _db = db;
        }

        public async Task<Account?> GetByIdAsync(int id) =>
            await _db.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Account?> GetByEmailAsync(string email) =>
            await _db.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        public async Task<List<Account>> GetAllAsync(Expression<Func<Account, bool>>? predicate = null)
        {
            IQueryable<Account> q = _db.Accounts.AsNoTracking();
            if (predicate != null) q = q.Where(predicate);
            return await q.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Account> CreateAsync(Account account)
        {
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();
            return account;
        }

        public async Task<Account> UpdateAsync(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
            return account;
        }

        // xóa cứng
        public async Task<bool> DeleteAsync(int id)
        {
            var acc = await _db.Accounts.FindAsync(id);
            if (acc == null) return false;

            _db.Accounts.Remove(acc);
            await _db.SaveChangesAsync();
            return true;
        }

        //xóa mềm
        public async Task<bool> DeactivateAsync(int id)
        {
            var acc = await _db.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            if (acc == null) return false;

            if (!acc.IsActive) return true;
            acc.IsActive = false;

            _db.Accounts.Update(acc);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
