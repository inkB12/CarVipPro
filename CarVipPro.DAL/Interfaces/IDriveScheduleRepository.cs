
using CarVipPro.DAL.Entities;

namespace CarVipPro.DAL.Interfaces
{
    public interface IDriveScheduleRepository
    {
        Task<List<DriveSchedule>> GetSchedulesByVehicleAndDateAsync(int vehicleId, DateTime date);
        Task<DriveSchedule> AddAsync(DriveSchedule schedule);
        Task<DriveSchedule> UpdateAsync(DriveSchedule schedule);
        Task<DriveSchedule?> GetDriveScheduleByIdAsync(int? id = 0);
        Task<List<DriveSchedule>> GetSchedulesByDateAsync(int vehicleId, DateTime date = default);
    }
}
