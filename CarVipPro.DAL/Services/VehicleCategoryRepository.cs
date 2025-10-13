using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.DAL.Services
{
    public class VehicleCategoryRepository : IVehicleCategoryRepository
    {
        private readonly CarVipProContext _context;
        public VehicleCategoryRepository(CarVipProContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<VehicleCategory>> GetAllAsync() => await _context.VehicleCategories.ToListAsync();
        public async Task<VehicleCategory> GetByIdAsync(int id) => await _context.VehicleCategories.FindAsync(id);
        public async Task AddAsync(VehicleCategory category)
        {
            _context.VehicleCategories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(VehicleCategory category)
        {
            _context.VehicleCategories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
           var entity = await _context.VehicleCategories.FindAsync(id);
            if (entity != null)
            {
                _context.VehicleCategories.Remove(entity);
                await _context.SaveChangesAsync();
            }

        }
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.VehicleCategories
                .AnyAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }
    }
    
}
