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
    public class CarCompanyRepository : ICarCompanyRepository
    {
        private readonly CarVipProContext _context;
        public CarCompanyRepository(CarVipProContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CarCompany>> GetAllAsync() => await _context.CarCompanies.ToListAsync();
        public async Task<CarCompany> GetByIdAsync(int id) => await _context.CarCompanies.FindAsync(id);
        public async Task AddAsync(CarCompany company)
        {
            _context.CarCompanies.Add(company);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(CarCompany company)
        {
            _context.CarCompanies.Update(company);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var company = await _context.CarCompanies.FindAsync(id);
            if (company != null)
            {
                _context.CarCompanies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }

    }
}
