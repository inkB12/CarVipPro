
using CarVipPro.BLL.Dtos;
using CarVipPro.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Interfaces
{
    public interface IElectricVehicleService
    {
        Task<List<ElectricVehicleDTO>> GetActiveByCompanyAsync(int companyId);
        Task<IEnumerable<ElectricVehicleDTO>> GetAll();
        Task<ElectricVehicleDTO> GetById(int id);
        Task Add(ElectricVehicleDTO vehicleDto);
        Task Update(ElectricVehicleDTO vehicleDto);
        Task Delete(int id);
    }
}
