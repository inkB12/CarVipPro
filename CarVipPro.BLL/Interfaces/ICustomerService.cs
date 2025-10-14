
using CarVipPro.BLL.Dtos;

namespace CarVipPro.BLL.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> SearchAsync(string keyword);
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CustomerDto dto);
    }
}
