
using CarVipPro.DAL.Entities;

namespace CarVipPro.DAL.Interfaces
{
    public interface IDriveScheduleRepository
    {
        Task<List<DriveSchedule>> GetSchedulesByVehicleAndDateAsync(int vehicleId, DateTime date);
        Task<DriveSchedule> AddAsync(DriveSchedule schedule);
        Task<List<DriveSchedule>> GetSchedulesByDateAsync(int vehicleId, DateTime date = default);
    }
}
