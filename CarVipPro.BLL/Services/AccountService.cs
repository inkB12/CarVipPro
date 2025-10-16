using System.Text;
using System.Security.Cryptography;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using CarVipPro.BLL.Dtos;

namespace CarVipPro.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;

        public AccountService(IAccountRepository repo)
        {
            _repo = repo;
        }

        // ===== Helpers =====
        private static AccountDTO ToDto(Account a) => new AccountDTO
        {
            Id = a.Id,
            Email = a.Email,
            Phone = a.Phone,
            FullName = a.FullName,
            Role = a.Role,
            IsActive = a.IsActive
        };

        private static void ApplyDto(Account entity, AccountDTO dto)
        {
            entity.Email = dto.Email?.Trim() ?? "";
            entity.FullName = dto.FullName?.Trim() ?? "";
            entity.Phone = dto.Phone;
            entity.Role = string.IsNullOrWhiteSpace(dto.Role) ? "Staff" : dto.Role;
            entity.IsActive = dto.IsActive;
        }

        private static string HashSHA256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        public async Task<List<AccountDTO>> GetAllAsync(bool onlyActive = true)
        {
            var list = onlyActive
                ? await _repo.GetAllAsync(a => a.IsActive)
                : await _repo.GetAllAsync();

            return list.Select(ToDto).ToList();
        }

        public async Task<AccountDTO?> GetByIdAsync(int id)
        {
            var acc = await _repo.GetByIdAsync(id);
            return acc is null ? null : ToDto(acc);
        }

        public async Task<(bool ok, string message, AccountDTO? data)> RegisterAsync(
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
            return (true, "Register success", ToDto(acc));
        }

        public async Task<AccountDTO?> LoginAsync(string email, string password)
        {
            var acc = await _repo.GetByEmailAsync(email);
            if (acc == null || !acc.IsActive) return null;
            if (HashSHA256(password) != acc.Password) return null;
            return ToDto(acc);
        }

        public async Task<(bool ok, string message, AccountDTO? data)> CreateAsync(AccountDTO dto, string password)
        {
            var email = dto.Email?.Trim() ?? "";
            var existed = await _repo.GetByEmailAsync(email);
            if (existed != null) return (false, "Email already exists", null);

            var entity = new Account
            {
                Email = email,
                FullName = dto.FullName?.Trim() ?? "",
                Phone = dto.Phone,
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "Staff" : dto.Role,
                IsActive = true,
                Password = HashSHA256(password)
            };

            var saved = await _repo.CreateAsync(entity);
            return (true, "Created", ToDto(saved));
        }

        public async Task<(bool ok, string message, AccountDTO? data)> UpdateAsync(AccountDTO dto, string? newPassword = null)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return (false, "Not found", null);

            if (!string.Equals(entity.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existed = await _repo.GetByEmailAsync(dto.Email);
                if (existed != null && existed.Id != entity.Id)
                    return (false, "Email already exists", null);
            }

            ApplyDto(entity, dto);
            if (!string.IsNullOrWhiteSpace(newPassword))
                entity.Password = HashSHA256(newPassword);

            var updated = await _repo.UpdateAsync(entity);
            return (true, "Updated", ToDto(updated));
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, "Deleted") : (false, "Not found");
        }
    }
}

