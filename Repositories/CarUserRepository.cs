using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.Entities;

namespace Repositories
{
    public class CarUserRepository
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public CarUserRepository(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        public async Task<CarUser> GetCarUserByUserId(int userId, int carId)
        {
            return await _context.CarUsers.FirstOrDefaultAsync(cu => cu.UserId == userId && cu.CarId == carId);
        }

        public async Task<CarUser> GetCarUserById(int? carUserId)
        {
            return await _context.CarUsers.FirstOrDefaultAsync(c => c.UserId == carUserId);
        }
    }
}
