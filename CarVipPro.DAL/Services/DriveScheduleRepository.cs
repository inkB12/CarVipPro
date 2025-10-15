

using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarVipPro.DAL.Services
{
    public class DriveScheduleRepository : IDriveScheduleRepository
    {

        private readonly CarVipProContext _context;

        public DriveScheduleRepository(CarVipProContext context)
        {
            _context = context;
        }

        // 📅 Lấy danh sách lịch theo xe & ngày
        public async Task<List<DriveSchedule>> GetSchedulesByVehicleAndDateAsync(int vehicleId, DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await _context.DriveSchedules
                .Include(ds => ds.Customer)
                .Where(ds => ds.ElectricVehicleId == vehicleId &&
                             ds.StartTime >= start && ds.StartTime < end)
                .OrderBy(ds => ds.StartTime)
                .ToListAsync();
        }

        // 📅 Lấy danh sách lịch theo ngày (Dynamic Searching)
        public async Task<List<DriveSchedule>> GetSchedulesByDateAsync(int vehicleId, DateTime date = default)
        {
            IQueryable<DriveSchedule> query = _context.DriveSchedules
                   .Include(ds => ds.Customer)
                   .Include(ds => ds.ElectricVehicle);

            if (vehicleId > 0)
            {
                query = query.Where(ds => ds.ElectricVehicleId == vehicleId);
            }

            if (date != default)
            {
                var start = date.Date;
                var end = start.AddDays(1);

                query = query.Where(ds => ds.StartTime >= start && ds.StartTime < end);
            }

            return await query
                .OrderBy(ds => ds.StartTime)
                .ToListAsync();
        }

        // ➕ Thêm mới lịch lái thử
        public async Task<DriveSchedule> AddAsync(DriveSchedule schedule)
        {
            _context.DriveSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }
    }
}
