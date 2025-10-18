using CarVipPro.DAL.Entities;

namespace CarVipPro.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task AddDetailsAsync(IEnumerable<OrderDetail> details);
        Task<Order?> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
        Task<int> SaveChangesAsync();
        Task<List<Order>> SearchAsync(string? q, string? status);
        Task<Order?> GetWithDetailsAsync(int id);
        Task<List<Order>> GetAllWithDetailsAsync();
    }
}
