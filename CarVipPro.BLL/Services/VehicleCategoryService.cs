using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Interfaces;
using System.Linq;

namespace CarVipPro.BLL.Services
{
    public class VehicleCategoryService : IVehicleCategoryService
    {
        private readonly IVehicleCategoryRepository _categoryRepo;
        private readonly IElectricVehicleRepository _vehicleRepo;

        public VehicleCategoryService(IVehicleCategoryRepository categoryRepo, IElectricVehicleRepository vehicleRepo)
        {
            _categoryRepo = categoryRepo;
            _vehicleRepo = vehicleRepo;
        }

        public async Task<IEnumerable<VehicleCategoryDTO>> GetAll()
        {
            var categories = await _categoryRepo.GetAllAsync();
            var vehicles = await _vehicleRepo.GetAllAsync();

            // Gộp dữ liệu và tính số lượng xe điện cho từng loại
            return categories.Select(c => new VehicleCategoryDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                IsActive = c.IsActive,
                ElectricVehicleCount = vehicles.Count(v => v.CategoryId == c.Id)
            }).ToList();
        }

        public async Task<VehicleCategoryDTO?> GetById(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return null;

            var vehicles = await _vehicleRepo.GetAllAsync();

            return new VehicleCategoryDTO
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive,
                ElectricVehicleCount = vehicles.Count(v => v.CategoryId == category.Id)
            };
        }

        public async Task Add(VehicleCategoryDTO dto)
        {
            var exists = await _categoryRepo.ExistsByNameAsync(dto.CategoryName);
            if (exists)
                throw new Exception("Tên loại xe đã tồn tại.");
            var entity = new CarVipPro.DAL.Entities.VehicleCategory
            {
                CategoryName = dto.CategoryName,
                IsActive = dto.IsActive
            };

            await _categoryRepo.AddAsync(entity);
        }

        public async Task Update(VehicleCategoryDTO dto)
        {
            var exists = await _categoryRepo.ExistsByNameAsync(dto.CategoryName);
            var current = await _categoryRepo.GetByIdAsync(dto.Id);

            if (exists && current.CategoryName.ToLower() != dto.CategoryName.ToLower())
                throw new Exception("Tên loại xe đã tồn tại.");

            current.CategoryName = dto.CategoryName;
            current.IsActive = dto.IsActive;

            await _categoryRepo.UpdateAsync(current);
        }
        public async Task Delete(int id)
        {
            await _categoryRepo.DeleteAsync(id);
        }
    }
}
