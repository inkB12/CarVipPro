
using CarVipPro.DAL.Entities;

namespace CarVipPro.DAL.Interfaces
{
    public interface IElectricVehicleRepository
    {
        Task<List<ElectricVehicle>> GetActiveByCompanyAsync(int companyId);
    }
}
