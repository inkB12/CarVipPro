
using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Interfaces;
namespace CarVipPro.BLL.Services
{
    public class ElectricVehicleService : IElectricVehicleService
    {
        private readonly IElectricVehicleRepository _vehicleRepo;

        public ElectricVehicleService(IElectricVehicleRepository vehicleRepo)
        {
            _vehicleRepo = vehicleRepo;
        }

        public async Task<List<ElectricVehicleDto>> GetActiveByCompanyAsync(int companyId)
        {
            var vehicles = await _vehicleRepo.GetActiveByCompanyAsync(companyId);
            return vehicles.Select(v => new ElectricVehicleDto
            {
                Id = v.Id,
                Model = v.Model,
                Color = v.Color
            }).ToList();
        }
    }
}
