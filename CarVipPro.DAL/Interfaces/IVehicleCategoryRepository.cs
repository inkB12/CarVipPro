using CarVipPro.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.DAL.Interfaces
{
    public interface IVehicleCategoryRepository
    {
        Task<IEnumerable<VehicleCategory>> GetAllAsync();
        Task<VehicleCategory> GetByIdAsync(int id);
        Task AddAsync(VehicleCategory category);
        Task UpdateAsync(VehicleCategory category);
        Task DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }

}
