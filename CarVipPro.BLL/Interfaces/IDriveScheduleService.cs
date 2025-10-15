

using CarVipPro.BLL.Dtos;

namespace CarVipPro.BLL.Interfaces
{
    public interface IDriveScheduleService
    {
        Task<List<DriveScheduleViewDto>> GetSchedulesByVehicleAndDateAsync(int vehicleId, DateTime date);
        Task<(bool Success, string Message, DriveScheduleViewDto? CreatedSchedule)> CreateAsync(DriveScheduleCreateDto dto);
        Task<List<DriveScheduleViewDto>> GetSchedulesByDateAsync(int vehicleId, DateTime date = default);
    }
}
