using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarVipPro.DAL.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CarVipProContext _db;
        public OrderRepository(CarVipProContext db) => _db = db;

        public async Task<Order> CreateAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task AddDetailsAsync(IEnumerable<OrderDetail> details)
        {
            await _db.OrderDetails.AddRangeAsync(details);
            await _db.SaveChangesAsync();
        }

        public Task<Order?> GetByIdAsync(int id) =>
            _db.Orders.AsNoTracking()
              .Include(o => o.OrderDetails)
              .FirstOrDefaultAsync(o => o.Id == id);

        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
