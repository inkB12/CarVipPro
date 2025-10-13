using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarVipPro.DAL.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CarVipProContext _db;
        public CustomerRepository(CarVipProContext db) => _db = db;

        public Task<Customer?> GetByIdAsync(int id) =>
            _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
}
