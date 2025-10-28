using Repositories;
using Repositories.DTOs.Car;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CarService
    {
        private readonly CarRepository _carRepository;  // Inject trực tiếp CarRepository

        public CarService(CarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        // Lấy tất cả xe
        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        // Lấy xe theo carId
        public async Task<Car> GetCarByIdAsync(int carId)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null) throw new KeyNotFoundException($"Car with id {carId} not found.");
            return car;
        }

        // Thêm xe mới
        public async Task<Car> CreateCarAsync(CarCreateDto dto)
        {
            var car = new Car
            {
                Brand = dto.Brand,
                CarName = dto.CarName,
                PlateNumber = dto.PlateNumber,
                Status = dto.Status,
                Image = dto.Image,
                Color = dto.Color,
                BatteryCapacity = dto.BatteryCapacity,
                CreatedAt = DateTime.UtcNow
            };

            await _carRepository.AddAsync(car);
            await _carRepository.SaveChangesAsync();
            return car;
        }

        // Cập nhật thông tin xe
        public async Task<Car> UpdateCarAsync(int carId, CarUpdateDto dto)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                throw new KeyNotFoundException($"Car with id {carId} not found.");

            // Kiểm tra duplicate plate number (nếu có thay đổi biển số)
            if (dto.PlateNumber != null && await _carRepository.PlateNumberExists(dto.PlateNumber, carId))
                throw new InvalidOperationException($"Plate number '{dto.PlateNumber}' already exists!");

            car.Brand = dto.Brand ?? car.Brand;
            car.CarName = dto.CarName ?? car.CarName;
            car.PlateNumber = dto.PlateNumber ?? car.PlateNumber;
            car.Status = dto.Status;
            car.Image = dto.Image ?? car.Image;
            car.Color = dto.Color ?? car.Color;
            car.BatteryCapacity = dto.BatteryCapacity;
            car.UpdatedAt = DateTime.UtcNow;

            await _carRepository.UpdateAsync(car);
            await _carRepository.SaveChangesAsync();

            return car;
        }


        // Xóa xe
        public async Task<bool> DeleteCarAsync(int carId)
        {
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null) return false;

            await _carRepository.DeleteAsync(car);
            await _carRepository.SaveChangesAsync();
            return true;
        }
    }
}

