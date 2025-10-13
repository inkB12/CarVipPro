using CarVipPro.BLL.Dtos;
using CarVipPro.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Interfaces
{
    public interface IVehicleCategoryService
    {
        Task<IEnumerable<VehicleCategoryDTO>> GetAll();
        Task<VehicleCategoryDTO> GetById(int id);
        Task Add(VehicleCategoryDTO categoryDto);
        Task Update(VehicleCategoryDTO categoryDto);
        Task Delete(int id);
    }
}
