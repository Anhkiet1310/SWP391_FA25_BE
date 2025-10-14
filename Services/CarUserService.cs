using Repositories;
using Repositories.Entities;

namespace Services
{
    public class CarUserService
    {
        private readonly CarUserRepository _carUserRepository;

        public CarUserService(CarUserRepository carUserRepository)
        {
            _carUserRepository = carUserRepository;
        }

        public async Task<CarUser> GetCarUserByUserId(int userId)
        {
            return await _carUserRepository.GetCarUserByUserId(userId);
        }
    }
}
