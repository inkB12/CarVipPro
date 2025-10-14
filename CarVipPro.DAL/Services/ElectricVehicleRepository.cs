
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CarVipPro.DAL.Services
{
    public class ElectricVehicleRepository : IElectricVehicleRepository
    {
        private readonly CarVipProContext _context;
        public ElectricVehicleRepository(CarVipProContext context)
        {
            _context = context;
        }

        public async Task<List<ElectricVehicle>> GetActiveByCompanyAsync(int companyId)
        {
            return await _context.ElectricVehicles
                .Where(v => v.CarCompanyId == companyId && v.IsActive)
                .ToListAsync();
        }
    }
}
