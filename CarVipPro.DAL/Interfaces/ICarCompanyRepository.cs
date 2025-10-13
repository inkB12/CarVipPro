using CarVipPro.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.DAL.Interfaces
{
    public interface ICarCompanyRepository
    {
        Task<IEnumerable<CarCompany>> GetAllAsync();
        Task<CarCompany> GetByIdAsync(int id);
        Task AddAsync(CarCompany company);
        Task UpdateAsync(CarCompany company);
        Task DeleteAsync(int id);
    }
}
