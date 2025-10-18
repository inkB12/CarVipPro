
using CarVipPro.DAL.Entities;

namespace CarVipPro.DAL.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> SearchAsync(string keyword);
        Task<Customer?> GetByIdWithDriveSchedulesAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);
        Task<List<Customer>> GetAllAsync();

    }
}
