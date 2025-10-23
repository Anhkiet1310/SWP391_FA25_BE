using AutoMapper;
using Repositories;
using Repositories.DTOs.User;
using Repositories.Entities;
using System.Text.RegularExpressions;

namespace Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(UserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                return false;
            return await _userRepository.DeleteUser(user);
        }

        public async Task<UserUpdateProfileDto> UpdateProfile(int id, UserUpdateProfileDto updateProfileDto)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            if (!IsValidEmail(updateProfileDto.Email))
                throw new Exception("Invalid email format");

            _mapper.Map(updateProfileDto, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateProfile(user);
            return _mapper.Map<UserUpdateProfileDto>(user);
        }

        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return _mapper.Map<List<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> Register(UserRegisterDto userRegisterDto)
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
                Email = userRegisterDto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var user = await _userRepository.Register(userEntity, userRegisterDto.Password);

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
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
