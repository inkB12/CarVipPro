
using CarVipPro.BLL.Dtos;
using CarVipPro.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Interfaces
{
    public interface ICarCompanyService
    {
        Task<List<CarCompanyDTO>> GetActiveCompaniesAsync();
        Task<IEnumerable<CarCompanyDTO>> GetAll();
        Task<CarCompanyDTO> GetById(int id);
        Task Add(CarCompanyDTO company);
        Task Update(CarCompanyDTO company);
        Task Delete(int id);
    }
}
