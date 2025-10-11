using System.Text;
using System.Security.Cryptography;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;

namespace CarVipPro.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;

        public AccountService(IAccountRepository repo)
        {
            _repo = repo;
        }

        // Get all: onlyActive = true => chỉ lấy tài khoản đang active
        public async Task<List<Account>> GetAllAsync(bool onlyActive = true)
        {
            if (onlyActive)
                return await _repo.GetAllAsync(a => a.IsActive == true);

            return await _repo.GetAllAsync();
        }

        public Task<Account?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<(bool ok, string message, Account? data)> RegisterAsync(
            string email, string password, string fullName, string? phone, string role = "Staff")
        {
            var existed = await _repo.GetByEmailAsync(email);
            if (existed != null) return (false, "Email already exists", null);

            var acc = new Account
            {
                Email = email.Trim(),
                FullName = fullName?.Trim() ?? "",
                Phone = phone,
                Password = HashSHA256(password),
                Role = string.IsNullOrWhiteSpace(role) ? "Staff" : role,
                IsActive = true
            };

            acc = await _repo.CreateAsync(acc);
            return (true, "Register success", acc);
        }

        // Login
        public async Task<(bool ok, string message, Account? data)> LoginAsync(string email, string password)
        {
            var acc = await _repo.GetByEmailAsync(email);
            if (acc == null) return (false, "Email not found", null);
            if (acc.IsActive == false) return (false, "Account is disabled", null);

            if (HashSHA256(password) != acc.Password)
                return (false, "Wrong Email or Password", null);

            return (true, "Login success", acc);
        }

        // Create (dùng cho admin tạo tài khoản)
        public async Task<(bool ok, string message, Account? data)> CreateAsync(Account account)
        {
            account.Email = account.Email?.Trim() ?? "";
            account.FullName = account.FullName?.Trim() ?? "";
            account.Password = HashSHA256(account.Password);
            account.IsActive = true;

            var existed = await _repo.GetByEmailAsync(account.Email);
            if (existed != null) return (false, "Email already exists", null);

            var saved = await _repo.CreateAsync(account);
            return (true, "Created", saved);
        }

        // Update (không hash lại password trừ khi bạn tự truyền password đã hash)
        public async Task<(bool ok, string message, Account? data)> UpdateAsync(Account account)
        {
            // account.Password = HashSHA256(account.Password);

            var updated = await _repo.UpdateAsync(account);
            return (true, "Updated", updated);
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, "Deleted") : (false, "Not found");
        }

        public static string HashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }
}
