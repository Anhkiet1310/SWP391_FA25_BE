using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.CarUser;
using Repositories.Entities;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarUserController : ControllerBase
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public CarUserController(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        // ✅ POST: /api/cars/{carId}/users/{userId}/add
        [HttpPost("/api/cars/{carId}/users/{userId}/add")]
        public async Task<IActionResult> AddUserToCar(int carId, int userId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                return NotFound($"Car with id {carId} not found.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"User with id {userId} not found.");

            var existing = await _context.CarUsers
                .FirstOrDefaultAsync(cUser => cUser.CarId == carId && cUser.UserId == userId && cUser.DeleteAt == null);

            if (existing != null)
                return Conflict($"User {userId} is already linked to Car {carId}.");

            var carUser = new CarUser
            {
                CarId = carId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.CarUsers.Add(carUser);
            await _context.SaveChangesAsync();

            var result = new CarUserDto
            {
                CarUserId = carUser.CarUserId,
                CarId = car.CarId,
                CarName = car.CarName,
                UserId = user.UserId,
                UserFullName = user.FullName,
                OwnershipPercentage = carUser.PercentOwnership?.Percentage,
                CreatedAt = DateTime.UtcNow
            };

            return Ok(result);
        }

        // ✅ DELETE: /api/cars/{carId}/users/{userId}/remove
        [HttpDelete("/api/cars/{carId}/users/{userId}/remove")]
        public async Task<IActionResult> RemoveUserFromCar(int carId, int userId)
        {
            var carUser = await _context.CarUsers
                .Include(cu => cu.Car)
                .Include(cu => cu.User)
                .FirstOrDefaultAsync(cUser => cUser.CarId == carId && cUser.UserId == userId && cUser.DeleteAt == null);

            if (carUser == null)
                return NotFound($"Relationship between Car {carId} and User {userId} not found.");

            carUser.DeleteAt = DateTime.UtcNow;
            _context.CarUsers.Update(carUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"User '{carUser.User.FullName}' removed from car '{carUser.Car.CarName}'.",
                RemovedAt = carUser.DeleteAt
            });
        }
    }
}