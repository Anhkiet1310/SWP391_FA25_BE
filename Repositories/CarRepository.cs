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
    public class CarRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public CarRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _context.Cars.Where(c => c.DeleteAt == null).ToListAsync();
        }

        public async Task<Car> GetByIdAsync(int carId)
        {
            return await _context.Cars.FindAsync(carId);
        }

        public async Task AddAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
        }

        public async Task UpdateAsync(Car car)
        {
            _context.Cars.Update(car);
        }

        public async Task DeleteAsync(Car car)
        {
            car.DeleteAt = DateTime.UtcNow;
            _context.Cars.Update(car);
        }

        public async Task<bool> PlateNumberExists(string plateNumber, int? excludeId = null)
        {
            return await _context.Cars
                .AnyAsync(c => c.PlateNumber == plateNumber && c.CarId != excludeId && c.DeleteAt == null);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

