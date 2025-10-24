using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.Entities;

namespace Repositories
{
    public class ScheduleRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;
        public ScheduleRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }
        public async Task<Schedule> CreateSchedule(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }
        public async Task<Schedule> UpdateSchedule(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }
        public async Task<Schedule?> GetById(int id)
        {
            return await _context.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == id && s.DeleteAt == null);
        }
        public async Task<List<Schedule>> GetAll()
        {
            return await _context.Schedules.Where(s => s.DeleteAt == null).ToListAsync();
        }
        public async Task<List<Schedule>> GetByUserId(int userId)
        {
            var schedules = await (from s in _context.Schedules
                                   join cu in _context.CarUsers on s.CarUserId equals cu.CarUserId
                                   where cu.UserId == userId
                                   select s).ToListAsync();

            return schedules;
        }

    }
}