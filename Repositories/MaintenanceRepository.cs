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
    public class MaintenanceRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public MaintenanceRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Maintenance>> GetAllAsync()
        {
            return await _context.Maintenances
                .Include(m => m.Car)  // Optional: Include related Car data
                .ToListAsync();
        }

        public async Task<Maintenance> AddMaintenanceAsync(Maintenance maintenance)
        {
            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();
            return maintenance;
        }

        public async Task<Maintenance?> GetByIdAsync(int id)
        {
            return await _context.Maintenances
                .Include(m => m.Car)
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);
        }

        public async Task<Maintenance?> DeleteMaintenanceAsync(int id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null) return null;

            _context.Maintenances.Remove(maintenance);
            await _context.SaveChangesAsync();
            return maintenance;
        }
    }
}
