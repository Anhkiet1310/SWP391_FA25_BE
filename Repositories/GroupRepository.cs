using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GroupRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public GroupRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _context.Groups.Where(g => g.DeleteAt == null).ToListAsync();
        }

        public async Task<Group?> GetByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Car)  // Include Car if needed
                .FirstOrDefaultAsync(g => g.GroupId == id && g.DeleteAt == null);
        }

        public async Task AddAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
        }

        public async Task UpdateAsync(Group group)
        {
            _context.Groups.Update(group);
        }

        public async Task DeleteAsync(Group group)
        {
            //group.DeleteAt = DateTime.UtcNow; // Soft delete
            //_context.Groups.Update(group);

            _context.Groups.Remove(group);  // Xóa hẳn khỏi DB
            await _context.SaveChangesAsync(); // Lưu thay đổi vào DB
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> GroupExists(int carId)
        {
            return await _context.Groups.AnyAsync(g => g.CarId == carId && g.DeleteAt == null);
        }
    }
}

