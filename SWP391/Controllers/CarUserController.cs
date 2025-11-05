using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.CarUser;
using Repositories.Entities;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarUserController : ControllerBase
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;
        private readonly CarUserService _carUserService;

        public CarUserController(Co_ownershipAndCost_sharingDbContext context, CarUserService carUserService)
        {
            _context = context;
            _carUserService = carUserService;
        }


        //[HttpGet("{userId}")]
        //public async Task<IActionResult> GetCarUserByUserId(int userId)
        //{
        //    if (userId <= 0)
        //    {
        //        return BadRequest("Invalid user ID");
        //    }
        //    try
        //    {
        //        var carUser = await _carUserService.GetCarUserByUserId(userId);
        //        if (carUser == null)
        //            return NotFound(new { message = "Not found!" });
        //        return Ok(carUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //// ✅ GET: /api/cars/{carId}/users - Lấy danh sách người dùng của một chiếc xe
        //[HttpGet("/api/cars/{carId}/users")]
        //public async Task<IActionResult> GetUsersByCarId(int carId)
        //{
        //    var carUsers = await _context.CarUsers
        //        .Include(cu => cu.User)  // Bao gồm thông tin người dùng
        //        .Where(cu => cu.CarId == carId && cu.DeleteAt == null) // Kiểm tra mối quan hệ chưa bị xóa
        //        .Select(cu => new CarUserDto
        //        {
        //            CarUserId = cu.CarUserId,
        //            CarId = cu.CarId,
        //            CarName = cu.Car.CarName,
        //            UserId = cu.UserId,
        //            UserFullName = cu.User.FullName,
        //            OwnershipPercentage = cu.PercentOwnership?.Percentage,
        //            CreatedAt = cu.CreatedAt
        //        })
        //        .ToListAsync();

        //    if (!carUsers.Any()) return NotFound($"No users found for Car {carId}.");

        //    return Ok(carUsers);
        //}

        // ✅ GET: /api/users/{userId}/cars - Lấy danh sách xe mà người dùng sở hữu hoặc sử dụng
        [HttpGet("/api/users/{userId}/cars")]
        public async Task<IActionResult> GetCarsByUserId(int userId)
        {
            var carUsers = await _context.CarUsers
                .Include(cu => cu.Car)  // Bao gồm thông tin xe
                .Include(cu => cu.PercentOwnership)  // Bao gồm thông tin tỷ lệ sở hữu
                .Where(cu => cu.UserId == userId && cu.DeleteAt == null)  // Kiểm tra mối quan hệ chưa bị xóa
                .Select(cu => new
                {
                    CarUserId = cu.CarUserId,
                    CarId = cu.CarId,
                    CarName = cu.Car.CarName,
                    PlateNumber = cu.Car.PlateNumber,
                    Status = cu.Car.Status,
                    BatteryCapacity = cu.Car.BatteryCapacity,
                    OwnershipPercentage = cu.PercentOwnership != null ? cu.PercentOwnership.Percentage : 0,  // Lấy tỷ lệ sở hữu từ PercentOwnership
                    CreatedAt = cu.CreatedAt
                })
                .ToListAsync();

            if (!carUsers.Any()) return NotFound($"No cars found for User {userId}.");

            return Ok(carUsers);
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
                .FirstOrDefaultAsync(cUser => cUser.CarId == carId && cUser.UserId == userId);

            if (existing != null)
                return Conflict($"User {userId} is already linked to Car {carId}.");

            try
            {
                // Add the new car-user relationship
                var carUser = new CarUser
                {
                    CarId = carId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.CarUsers.Add(carUser);
                await _context.SaveChangesAsync();

                // Return the result
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
            catch (Exception ex)
            {
                // Re-throw if it's a different type of error
                throw;
            }
        }

        // ✅ DELETE: /api/cars/{carId}/users/{userId}/remove
        [HttpDelete("/api/cars/{carId}/users/{userId}/remove")]
        public async Task<IActionResult> RemoveUserFromCar(int carId, int userId)
        {
            var carUser = await _context.CarUsers
                .Include(cu => cu.Car)
                .Include(cu => cu.User)
                .FirstOrDefaultAsync(cUser => cUser.CarId == carId && cUser.UserId == userId);

            if (carUser == null)
                return NotFound($"Relationship between Car {carId} and User {userId} not found.");

            //carUser.DeleteAt = DateTime.UtcNow;
            //_context.CarUsers.Update(carUser);
            //await _context.SaveChangesAsync();

            _context.CarUsers.Remove(carUser);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = $"User '{carUser.User.FullName}' removed from car '{carUser.Car.CarName}'.",
                RemovedAt = carUser.DeleteAt
            });
        }
    }
}