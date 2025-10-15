

using CarVipPro.BLL.Dtos;

namespace CarVipPro.BLL.Interfaces
{
    public interface IDriveScheduleService
    {
        Task<List<DriveScheduleViewDto>> GetSchedulesByVehicleAndDateAsync(int vehicleId, DateTime date);
        Task<(bool Success, string Message, DriveScheduleViewDto? CreatedSchedule)> CreateAsync(DriveScheduleCreateDto dto);
        Task<List<DriveScheduleViewDto>> GetSchedulesByDateAsync(int vehicleId, DateTime date = default);
        Task<(bool Success, string Message, DriveScheduleViewDto? UpdatedSchedule)> UpdateSchedule(DriveScheduleCreateDto dto, int? driveScheduleId = 0);
        Task<DriveScheduleCreateDto> GetScheduleByIdAsync(int scheduleId);
        Task<DriveScheduleViewDto> GetViewScheduleByIdAsync(int scheduleId);
    }
}
