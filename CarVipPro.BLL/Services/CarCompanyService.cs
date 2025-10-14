
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Interfaces;
namespace CarVipPro.BLL.Services
{
    public class CarCompanyService : ICarCompanyService
    {
        private readonly ICarCompanyRepository _carCompanyRepo;

        public CarCompanyService(ICarCompanyRepository carCompanyRepo)
        {
            _carCompanyRepo = carCompanyRepo;
        }

        public async Task<List<CarCompanyDto>> GetActiveCompaniesAsync()
        {
            var companies = await _carCompanyRepo.GetActiveAsync();
            return companies.Select(c => new CarCompanyDto
            {
                Id = c.Id,
                CatalogName = c.CatalogName
            }).ToList();
        }
    }
}
