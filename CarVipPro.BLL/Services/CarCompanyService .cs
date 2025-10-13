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
    public class CarCompanyService : ICarCompanyService
    {
        private readonly ICarCompanyRepository _repo;

        public CarCompanyService(ICarCompanyRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Lấy danh sách tất cả hãng xe và ánh xạ sang DTO
        /// </summary>
        public async Task<IEnumerable<CarCompanyDTO>> GetAll()
        {
            var companies = await _repo.GetAllAsync();

            return companies.Select(c => new CarCompanyDTO
            {
                Id = c.Id,
                CatalogName = c.CatalogName,
                Description = c.Description,
                IsActive = c.IsActive,
                ElectricVehicleCount = c.ElectricVehicles?.Count ?? 0
            });
        }

        /// <summary>
        /// Lấy thông tin chi tiết hãng xe theo ID
        /// </summary>
        public async Task<CarCompanyDTO> GetById(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new CarCompanyDTO
            {
                Id = c.Id,
                CatalogName = c.CatalogName,
                Description = c.Description,
                IsActive = c.IsActive,
                ElectricVehicleCount = c.ElectricVehicles?.Count ?? 0
            };
        }

        /// <summary>
        /// Thêm mới hãng xe (DTO → Entity)
        /// </summary>
        public async Task Add(CarCompanyDTO companyDto)
        {
            var entity = new CarCompany
            {
                CatalogName = companyDto.CatalogName,
                Description = companyDto.Description,
                IsActive = companyDto.IsActive
            };

            await _repo.AddAsync(entity);
        }

        /// <summary>
        /// Cập nhật hãng xe (DTO → Entity)
        /// </summary>
        public async Task Update(CarCompanyDTO companyDto)
        {
            var entity = new CarCompany
            {
                Id = companyDto.Id,
                CatalogName = companyDto.CatalogName,
                Description = companyDto.Description,
                IsActive = companyDto.IsActive
            };

            await _repo.UpdateAsync(entity);
        }

        /// <summary>
        /// Xóa hãng xe theo ID
        /// </summary>
        public async Task Delete(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
