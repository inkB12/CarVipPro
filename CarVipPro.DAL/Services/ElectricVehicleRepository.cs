using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarVipPro.DAL.Services
{
    public class ElectricVehicleRepository : IElectricVehicleRepository
    {
        private readonly CarVipProContext _context;

        public ElectricVehicleRepository(CarVipProContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ElectricVehicle>> GetAllAsync()
        {
            return await _context.ElectricVehicles
                .Include(v => v.CarCompany)       // 🟢 Join sang bảng Hãng xe
                .Include(v => v.Category)         // 🟢 Join sang bảng Loại xe
                .ToListAsync();
        }

        public async Task<ElectricVehicle?> GetByIdAsync(int id)
        {
            return await _context.ElectricVehicles
                .Include(v => v.CarCompany)
                .Include(v => v.Category)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(ElectricVehicle entity)
        {
            _context.ElectricVehicles.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ElectricVehicle entity)
        {
            _context.ElectricVehicles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ElectricVehicles.FindAsync(id);
            if (entity != null)
            {
                _context.ElectricVehicles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByModelAsync(string model)
        {
            return await _context.ElectricVehicles.AnyAsync(v => v.Model == model);
        }

        public async Task<Dictionary<int, ElectricVehicle>> GetActiveByIdsAsync(IEnumerable<int> ids)
        {
            var list = await _context.ElectricVehicles.AsNoTracking()
                          .Where(e => ids.Contains(e.Id) && e.IsActive)
                          .ToListAsync();
            return list.ToDictionary(e => e.Id);
        }
    }
}
