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
                .Include(ev => ev.CarCompany)
                .Include(ev => ev.Category) // Corrected property name from VehicleCategory to Category  
                .ToListAsync();
        }

        public async Task<ElectricVehicle> GetByIdAsync(int id)
        {
            return await _context.ElectricVehicles
                .Include(ev => ev.CarCompany)
                .Include(ev => ev.Category) // Corrected property name from VehicleCategory to Category  
                .FirstOrDefaultAsync(ev => ev.Id == id);
        }

        public async Task AddAsync(ElectricVehicle vehicle)
        {
            await _context.ElectricVehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ElectricVehicle vehicle)
        {
            _context.ElectricVehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ElectricVehicles.FirstOrDefaultAsync(ev => ev.Id == id);
            if (entity != null)
            {
                _context.ElectricVehicles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
