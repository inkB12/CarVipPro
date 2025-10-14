using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Interfaces;
using System.Linq;

namespace CarVipPro.BLL.Services
{
    public class CarCompanyService : ICarCompanyService
    {
        private readonly ICarCompanyRepository _companyRepo;
        private readonly IElectricVehicleRepository _vehicleRepo;

        public CarCompanyService(ICarCompanyRepository companyRepo, IElectricVehicleRepository vehicleRepo)
        {
            _companyRepo = companyRepo;
            _vehicleRepo = vehicleRepo;
        }

        public async Task<List<CarCompanyDTO>> GetActiveCompaniesAsync()
        {
            var companies = await _companyRepo.GetActiveAsync();
            return companies.Select(c => new CarCompanyDTO
            {
                Id = c.Id,
                CatalogName = c.CatalogName
            }).ToList();
        }

        public async Task<IEnumerable<CarCompanyDTO>> GetAll()
        {
            var companies = await _companyRepo.GetAllAsync();
            var vehicles = await _vehicleRepo.GetAllAsync();

            // Gộp dữ liệu và tính số lượng xe điện cho từng hãng
            return companies.Select(c => new CarCompanyDTO
            {
                Id = c.Id,
                CatalogName = c.CatalogName,
                Description = c.Description,
                IsActive = c.IsActive,
                ElectricVehicleCount = vehicles.Count(v => v.CarCompanyId == c.Id)
            }).ToList();
        }

        public async Task<CarCompanyDTO?> GetById(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null) return null;

            var vehicles = await _vehicleRepo.GetAllAsync();

            return new CarCompanyDTO
            {
                Id = company.Id,
                CatalogName = company.CatalogName,
                Description = company.Description,
                IsActive = company.IsActive,
                ElectricVehicleCount = vehicles.Count(v => v.CarCompanyId == company.Id)
            };
        }

        public async Task Add(CarCompanyDTO dto)
        {
            var exists = await _companyRepo.ExistsByNameAsync(dto.CatalogName);
            if (exists)
                throw new Exception("Tên hãng xe đã tồn tại.");
            var entity = new CarVipPro.DAL.Entities.CarCompany
            {
                CatalogName = dto.CatalogName,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            await _companyRepo.AddAsync(entity);
        }

        public async Task Update(CarCompanyDTO dto)
        {
            var entity = await _companyRepo.GetByIdAsync(dto.Id);
            if (entity == null) return;

            entity.CatalogName = dto.CatalogName;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;

            await _companyRepo.UpdateAsync(entity);
        }

        public async Task Delete(int id)
        {
            await _companyRepo.DeleteAsync(id);
        }

    }
}
