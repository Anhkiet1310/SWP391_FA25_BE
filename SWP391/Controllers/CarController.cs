using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.DBContext;
using Repositories.DTOs.Car;
using Repositories.Entities;
using Services;



namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarService _carService;  // Inject trực tiếp CarService

        // Constructor: Inject CarService trực tiếp vào Controller
        public CarController(CarService carService)
        {
            _carService = carService;
        }

        // ✅ GET: api/cars
        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            try
            {
                var cars = await _carService.GetAllCarsAsync();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Handle errors
            }
        }

        // ✅ POST: api/cars
        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] CarCreateDto carDto)
        {
            try
            {
                var car = await _carService.CreateCarAsync(carDto);
                return CreatedAtAction(nameof(GetCarById), new { id = car.CarId }, car);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message); // Handle conflict (e.g., duplicate plate number)
            }
        }

        // ✅ GET: api/cars/{carId} (dùng cho CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound($"Car with id {id} not found.");
            return Ok(car);
        }

        // PUT: api/cars/{carId}/update
        [HttpPut("{carId}/update")]
        public async Task<IActionResult> UpdateCar(int carId, [FromBody] CarUpdateDto dto)
        {
            try
            {
                var updatedCar = await _carService.UpdateCarAsync(carId, dto);
                return Ok(updatedCar);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Handle not found
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // Handle conflicts (duplicate plate number)
            }
        }

        // ✅ DELETE: api/cars/{carId}/delete
        [HttpDelete("{carId}/delete")]
        public async Task<IActionResult> DeleteCar(int carId)
        {
            var result = await _carService.DeleteCarAsync(carId);
            if (!result)
                return NotFound($"Car with id {carId} not found.");

            return Ok($"Car with id {carId} has been deleted.");
        }
    }
}

//namespace SWP391.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CarController : ControllerBase
//    {
//        private readonly Co_ownershipAndCost_sharingDbContext _context;

//        public CarController(Co_ownershipAndCost_sharingDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/cars
//        [HttpGet]
//        public async Task<IActionResult> GetCars()
//        {
//            var cars = await _context.Cars.ToListAsync();
//            return Ok(cars);
//        }

//        // POST: api/cars
//        [HttpPost]
//        public async Task<IActionResult> CreateCar([FromBody] CarCreateDto carDto)
//        {
//            if (carDto == null) return BadRequest("Car data is required");

//            var car = new Car
//            {
//                Brand = carDto.Brand,
//                CarName = carDto.CarName,
//                PlateNumber = carDto.PlateNumber,
//                Status = carDto.Status,
//                Image = carDto.Image,
//                Color = carDto.Color,
//                BatteryCapacity = carDto.BatteryCapacity,
//                CreatedAt = DateTime.UtcNow
//            };

//            _context.Cars.Add(car);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetCarById), new { id = car.CarId }, car);
//        }

//        // GET: api/cars/{id} (dùng cho CreatedAtAction)
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetCarById(int id)
//        {
//            var car = await _context.Cars.FindAsync(id);
//            if (car == null) return NotFound();
//            return Ok(car);
//        }

//        [HttpGet("test")]
//        public async Task<IActionResult> Test()
//        {
//            return Ok("test 3");
//        }


//    }
//}
