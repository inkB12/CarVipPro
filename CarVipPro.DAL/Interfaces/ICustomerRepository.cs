using CarVipPro.DAL.Entities;

namespace CarVipPro.DAL.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(int id);
    }
}
