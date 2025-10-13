using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Services
{
    public class VehicleCategoryService : IVehicleCategoryService
    {
        private readonly IVehicleCategoryRepository _repo;

        public VehicleCategoryService(IVehicleCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<VehicleCategoryDTO>> GetAll()
        {
            var categories = await _repo.GetAllAsync();

            return categories.Select(c => new VehicleCategoryDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                IsActive = c.IsActive,
                ElectricVehicleCount = c.ElectricVehicles?.Count ?? 0
            });
        }

        public async Task<VehicleCategoryDTO> GetById(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new VehicleCategoryDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                IsActive = c.IsActive,
                ElectricVehicleCount = c.ElectricVehicles?.Count ?? 0
            };
        }

        public async Task Add(VehicleCategoryDTO categoryDto)
        {
            var category = new VehicleCategory
            {
                CategoryName = categoryDto.CategoryName,
                IsActive = categoryDto.IsActive
            };

            await _repo.AddAsync(category);
        }

        public async Task Update(VehicleCategoryDTO categoryDto)
        {
            var category = new VehicleCategory
            {
                Id = categoryDto.Id,
                CategoryName = categoryDto.CategoryName,
                IsActive = categoryDto.IsActive
            };

            await _repo.UpdateAsync(category);
        }

        public async Task Delete(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
