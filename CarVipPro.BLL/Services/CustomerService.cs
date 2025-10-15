

using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;

namespace CarVipPro.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        // 🔍 Tìm kiếm khách hàng
        public async Task<List<CustomerDto>> SearchAsync(string keyword)
        {
            var customers = await _customerRepo.SearchAsync(keyword);

            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                Phone = c.Phone,
                IdentityCard = c.IdentityCard,
                Address = c.Address,
                ZipCode = c.ZipCode
            }).ToList();
        }

        // 🔁 Lấy chi tiết khách hàng + lịch lái thử cũ
        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepo.GetByIdWithDriveSchedulesAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                IdentityCard = customer.IdentityCard,
                Address = customer.Address,
                ZipCode = customer.ZipCode,
                DriveSchedules = customer.DriveSchedules.Select(ds => new DriveScheduleSummaryDto
                {
                    Id = ds.Id,
                    VehicleModel = ds.ElectricVehicle.Model,
                    StartTime = ds.StartTime,
                    EndTime = ds.EndTime,
                    Status = ds.Status
                }).OrderByDescending(d => d.StartTime).ToList()
            };
        }

        // ➕ Thêm mới khách hàng
        public async Task<CustomerDto> CreateAsync(CustomerDto dto)
        {
            var entity = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                IdentityCard = dto.IdentityCard,
                Address = dto.Address,
                ZipCode = dto.ZipCode
            };

            var added = await _customerRepo.AddAsync(entity);

            dto.Id = added.Id;
            return dto;
        }
    }
}
