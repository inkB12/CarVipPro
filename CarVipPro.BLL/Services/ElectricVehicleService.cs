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
    public class ElectricVehicleService : IElectricVehicleService
    {
        private readonly IElectricVehicleRepository _repo;

        public ElectricVehicleService(IElectricVehicleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ElectricVehicleDTO>> GetAll()
        {
            var vehicles = await _repo.GetAllAsync();
            return vehicles.Select(v => new ElectricVehicleDTO
            {
                Id = v.Id,
                Model = v.Model,
                Price = v.Price,
           
                ImageUrl = v.ImageUrl,
                Specification = v.Specification,
                IsActive = v.IsActive,
                CarCompanyId = v.CarCompanyId,
                CategoryId = v.CategoryId,
                CarCompanyName = v.CarCompany?.CatalogName,
                CategoryName = v.Category?.CategoryName
            });
        }

        public async Task<ElectricVehicleDTO> GetById(int id)
        {
            var v = await _repo.GetByIdAsync(id);
            if (v == null) return null;

            return new ElectricVehicleDTO
            {
                Id = v.Id,
                Model = v.Model,
                Price = v.Price,
              
                ImageUrl = v.ImageUrl,
                Specification = v.Specification,
                IsActive = v.IsActive,
                CarCompanyId = v.CarCompanyId,
                CategoryId = v.CategoryId,
                CarCompanyName = v.CarCompany?.CatalogName,
                CategoryName = v.Category?.CategoryName
            };
        }

        public async Task Add(ElectricVehicleDTO vehicleDto)
        {
            var vehicle = new ElectricVehicle
            {
                Model = vehicleDto.Model,
                Price = vehicleDto.Price,
            
                ImageUrl = vehicleDto.ImageUrl,
                Specification = vehicleDto.Specification,
                IsActive = vehicleDto.IsActive,
                CarCompanyId = vehicleDto.CarCompanyId,
                CategoryId = vehicleDto.CategoryId
            };

            await _repo.AddAsync(vehicle);
        }

        public async Task Update(ElectricVehicleDTO vehicleDto)
        {
            var vehicle = new ElectricVehicle
            {
                Id = vehicleDto.Id,
                Model = vehicleDto.Model,
                Price = vehicleDto.Price,
     
                ImageUrl = vehicleDto.ImageUrl,
                Specification = vehicleDto.Specification,
                IsActive = vehicleDto.IsActive,
                CarCompanyId = vehicleDto.CarCompanyId,
                CategoryId = vehicleDto.CategoryId
            };

            await _repo.UpdateAsync(vehicle);
        }

        public async Task Delete(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
