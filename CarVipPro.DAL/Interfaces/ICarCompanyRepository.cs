
using CarVipPro.DAL.Entities;

namespace CarVipPro.DAL.Interfaces
{
    public interface ICarCompanyRepository
    {
        Task<List<CarCompany>> GetActiveAsync();
    }
}
