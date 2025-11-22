using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DBContext;
using Repositories.DTOs.Maintenance;
using Repositories.Entities;
using Repositories.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MaintenanceService
    {
        private readonly MaintenanceRepository _maintenanceRepo;
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public MaintenanceService(MaintenanceRepository maintenanceRepo, Co_ownershipAndCost_sharingDbContext context)
        {
            _maintenanceRepo = maintenanceRepo;
            _context = context;
        }

        public async Task<IEnumerable<Maintenance>> GetAllMaintenanceAsync()
        {
            return await _context.Maintenances
                .Include(m => m.Car)  // Optional: if you want to include car details as well
                .ToListAsync();
        }


        public async Task<Maintenance?> CreateMaintenanceAsync(MaintenanceCreateDto dto)
        {
            var car = await _context.Cars.FindAsync(dto.CarId);
            if (car == null)
                return null;

            var maintenance = new Maintenance
            {
                CarId = dto.CarId,
                MaintenanceType = dto.MaintenanceType,
                MaintenanceDay = dto.MaintenanceDay,
                Status = (MaintenanceStatus)dto.Status,
                Description = dto.Description,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow
            };

            return await _maintenanceRepo.AddMaintenanceAsync(maintenance);
        }

        public async Task<Maintenance?> GetMaintenanceByIdAsync(int id)
        {
            return await _maintenanceRepo.GetByIdAsync(id);
        }

        public async Task<Maintenance?> UpdateMaintenanceAsync(int id, MaintenanceCreateDto dto)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null)
                return null;

            // Cập nhật giá trị từ DTO vào entity
            maintenance.MaintenanceType = dto.MaintenanceType;
            maintenance.MaintenanceDay = dto.MaintenanceDay;
            maintenance.Status = (MaintenanceStatus)dto.Status;
            maintenance.Description = dto.Description;
            maintenance.Price = dto.Price;
            maintenance.UpdatedAt = DateTime.UtcNow;

            return await _maintenanceRepo.UpdateMaintenanceAsync(id, maintenance);
        }


        public async Task<Maintenance?> DeleteMaintenanceAsync(int id)
        {
            return await _maintenanceRepo.DeleteMaintenanceAsync(id);
        }
    }
}
