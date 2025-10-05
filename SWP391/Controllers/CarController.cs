using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.Car;
using Repositories.Entities;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly Co_ownershipAndCost_sharingDbContext _context;

        public CarController(Co_ownershipAndCost_sharingDbContext context)
        {
            _context = context;
        }

        // GET: api/cars
        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _context.Cars.ToListAsync();
            return Ok(cars);
        }

        // POST: api/cars
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CarCreateDto carDto)
        {
            if (carDto == null) return BadRequest("Car data is required");

            var car = new Car
            {
                Brand = carDto.Brand,
                CarName = carDto.CarName,
                PlateNumber = carDto.PlateNumber,
                Status = carDto.Status,
                Image = carDto.Image,
                Color = carDto.Color,
                BatteryCapacity = carDto.BatteryCapacity,
                CreatedAt = DateTime.UtcNow
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCarById), new { id = car.CarId }, car);
        }

        // GET: api/cars/{id} (dùng cho CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }
    }
}
