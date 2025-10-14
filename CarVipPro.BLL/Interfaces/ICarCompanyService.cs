
using CarVipPro.BLL.Dtos;
namespace CarVipPro.BLL.Interfaces
{
    public interface ICarCompanyService
    {
        Task<List<CarCompanyDto>> GetActiveCompaniesAsync();
    }
}
