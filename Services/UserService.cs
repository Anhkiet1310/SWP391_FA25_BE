using Repositories;
using Repositories.DTOs.User;
using Repositories.Entities;
using System.Text.RegularExpressions;

namespace Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserRegisterDto> Register(UserRegisterDto userRegisterDto)
        {
            var existingUser = await _userRepository.GetUserByUsername(userRegisterDto.UserName);
            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }

            if (!IsValidEmail(userRegisterDto.Email))
            {
                throw new Exception("Invalid email format");
            }

            var userEntity = new User
            {
                UserName = userRegisterDto.UserName,
                FullName = userRegisterDto.FullName,
                Email = userRegisterDto.Email
            };

            var user = await _userRepository.Register(userEntity, userRegisterDto.Password);

            return new UserRegisterDto
            {
                UserName = user.UserName,
                Password = user.PasswordHash,
                Email = user.Email,
                FullName = user.FullName
            };
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _userRepository.Login(username, password);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }
            return user;
        }

        private bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) 
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
    }
}
