
using CarVipPro.BLL.Dtos;
namespace CarVipPro.BLL.Interfaces
{
    public interface IElectricVehicleService
    {
        Task<List<ElectricVehicleDto>> GetActiveByCompanyAsync(int companyId);
    }
}
