using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Services
{
    public class ElectricVehicleService : IElectricVehicleService
    {
        private readonly IElectricVehicleRepository _vehicleRepo;

        public ElectricVehicleService(IElectricVehicleRepository vehicleRepo)
        {
            _vehicleRepo = vehicleRepo;
        }

        public async Task<IEnumerable<ElectricVehicleDTO>> GetAll()
        {
            var vehicles = await _vehicleRepo.GetAllAsync();

            return vehicles.Select(v => new ElectricVehicleDTO
            {
                Id = v.Id,
                Model = v.Model,
                Version = v.Version,
                Price = v.Price,
                Color = v.Color,
                Specification = v.Specification,
                ImageUrl = v.ImageUrl,
                IsActive = v.IsActive,
                CarCompanyId = v.CarCompanyId,
                CategoryId = v.CategoryId,

                // 🟢 Bổ sung 2 dòng này
                CarCompanyName = v.CarCompany != null ? v.CarCompany.CatalogName : "Không rõ",
                CategoryName = v.Category != null ? v.Category.CategoryName : "Không rõ"
            });
        }

        public async Task<ElectricVehicleDTO?> GetById(int id)
        {
            var v = await _vehicleRepo.GetByIdAsync(id);
            if (v == null) return null;

            return new ElectricVehicleDTO
            {
                Id = v.Id,
                Model = v.Model,
                Version = v.Version,
                Price = v.Price,
                Color = v.Color,
                Specification = v.Specification,
                ImageUrl = v.ImageUrl,
                IsActive = v.IsActive,
                CarCompanyId = v.CarCompanyId,
                CategoryId = v.CategoryId
            };
        }

        public async Task Add(ElectricVehicleDTO dto)
        {
            var exists = await _vehicleRepo.ExistsByModelAsync(dto.Model);
            if (exists)
                throw new Exception("Model xe điện đã tồn tại.");

            var entity = new ElectricVehicle
            {
                Model = dto.Model,
                Version = dto.Version,
                Price = dto.Price,
                Color = dto.Color,
                Specification = dto.Specification,
                ImageUrl = dto.ImageUrl,
                IsActive = dto.IsActive,
                CarCompanyId = dto.CarCompanyId,
                CategoryId = dto.CategoryId
            };

            await _vehicleRepo.AddAsync(entity);
        }

        public async Task Update(ElectricVehicleDTO dto)
        {
            var current = await _vehicleRepo.GetByIdAsync(dto.Id);
            if (current == null)
                throw new Exception("Không tìm thấy xe điện.");

            var exists = await _vehicleRepo.ExistsByModelAsync(dto.Model);
            if (exists && !string.Equals(current.Model, dto.Model, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Model xe điện đã tồn tại.");

            current.Model = dto.Model;
            current.Version = dto.Version;
            current.Price = dto.Price;
            current.Color = dto.Color;
            current.Specification = dto.Specification;
            current.ImageUrl = dto.ImageUrl;
            current.CarCompanyId = dto.CarCompanyId;
            current.CategoryId = dto.CategoryId;
            current.IsActive = dto.IsActive;

            await _vehicleRepo.UpdateAsync(current);
        }

        public async Task Delete(int id)
        {
            await _vehicleRepo.DeleteAsync(id);
        }
    }
}
