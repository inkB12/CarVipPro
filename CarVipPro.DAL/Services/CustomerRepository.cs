
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarVipPro.DAL.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CarVipProContext _context;

        public CustomerRepository(CarVipProContext context)
        {
            _context = context;
        }

        // 🔍 Search theo nhiều field: Họ tên, Email, SĐT, CMND
        public async Task<List<Customer>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<Customer>();
            }

            keyword = keyword.Trim().ToLower();

            return await _context.Customers
                .Where(c =>
                    c.FullName.ToLower().Contains(keyword) ||
                    c.Email.ToLower().Contains(keyword) ||
                    (c.Phone != null && c.Phone.Contains(keyword)) ||
                    (c.IdentityCard != null && c.IdentityCard.Contains(keyword)))
                .AsNoTracking()
                .ToListAsync();
        }

        // 🔁 Lấy chi tiết khách hàng + lịch lái thử trước đó
        public async Task<Customer?> GetByIdWithDriveSchedulesAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.DriveSchedules)
                    .ThenInclude(ds => ds.ElectricVehicle)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // ➕ Thêm mới khách hàng
        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public Task<Customer?> GetByIdAsync(int id) =>
            _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }
}
