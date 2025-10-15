
using CarVipPro.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.DAL.Interfaces
{
    public interface IElectricVehicleRepository
    {
        Task<List<ElectricVehicle>> GetActiveByCompanyAsync(int companyId);
        Task<IEnumerable<ElectricVehicle>> GetAllAsync();
        Task<ElectricVehicle> GetByIdAsync(int id);
        Task AddAsync(ElectricVehicle vehicle);
        Task UpdateAsync(ElectricVehicle vehicle);
        Task DeleteAsync(int id);
        Task<bool> ExistsByModelAsync(string model);

        Task<Dictionary<int, ElectricVehicle>> GetActiveByIdsAsync(IEnumerable<int> ids);
    }
}
