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

        public async Task<List<Order>> SearchAsync(string? q, string? status)
        {
            var query = _db.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .OrderByDescending(o => o.DateTime)
            .AsQueryable();


            if (!string.IsNullOrWhiteSpace(status)) query = query.Where(o => o.Status == status);
            if (!string.IsNullOrWhiteSpace(q))
            {
                var k = q.Trim();
                query = query.Where(o => o.Id.ToString().Contains(k) || o.Customer.FullName.Contains(k) || o.Customer.Email.Contains(k));
            }
            return await query.ToListAsync();
        }

        public async Task<Order?> GetWithDetailsAsync(int id)
        {
            return await _db.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.OrderDetails).ThenInclude(od => od.ElectricVehicle).ThenInclude(ev => ev.CarCompany)
            .Include(o => o.OrderDetails).ThenInclude(od => od.ElectricVehicle).ThenInclude(ev => ev.Category)
            .FirstOrDefaultAsync(o => o.Id == id);
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
