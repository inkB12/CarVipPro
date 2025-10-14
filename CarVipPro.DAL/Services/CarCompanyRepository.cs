
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CarVipPro.DAL.Services
{
    public class CarCompanyRepository : ICarCompanyRepository
    {
        private readonly CarVipProContext _context;
        public CarCompanyRepository(CarVipProContext context)
        {
            _context = context;
        }

        public async Task<List<CarCompany>> GetActiveAsync()
        {
            return await _context.CarCompanies
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }
}
