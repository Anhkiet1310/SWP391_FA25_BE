using AutoMapper;
using Repositories;
using Repositories.DTOs.User;
using Repositories.Entities;
using Services.Utils;
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

        public async Task<ServiceResult<UserUpdateProfileDto>> UpdateProfile(int id, UserUpdateProfileDto updateProfileDto)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                return new ServiceResult<UserUpdateProfileDto>
                {
                    Success = false,
                    Message = "User not found"
                };

            var result = await IsValidEmail(updateProfileDto.Email, true);
            if (result != null)
                return new ServiceResult<UserUpdateProfileDto>
                {
                    Success = false,
                    Message = result
                };

            _mapper.Map(updateProfileDto, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateProfile(user);
            var userUpdateProfileDto =  _mapper.Map<UserUpdateProfileDto>(user);

            return new ServiceResult<UserUpdateProfileDto>
            {
                Success = true,
                Data = userUpdateProfileDto
            };
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

        public async Task<ServiceResult<UserResponseDto>> Register(UserRegisterDto userRegisterDto)
        {
            var resultUserName = await IsValidUserName(userRegisterDto.UserName);
            if (resultUserName != null)
                return new ServiceResult<UserResponseDto>
                {
                    Success = false,
                    Message = resultUserName
                };

            var resultEmail = await IsValidEmail(userRegisterDto.Email, false);
            if (resultEmail != null)
                return new ServiceResult<UserResponseDto>
                {
                    Success = false,
                    Message = resultEmail
                };

            var userEntity = new User
            {
                UserName = userRegisterDto.UserName,
                FullName = userRegisterDto.FullName,
                Email = userRegisterDto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var user = await _userRepository.Register(userEntity, userRegisterDto.Password);

            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return new ServiceResult<UserResponseDto>
            {
                Success = true,
                Data = userResponseDto
            };
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

        private async Task<string?> IsValidEmail(string? email, bool update)
        {
            if (string.IsNullOrEmpty(email)) 
                return "Email are required";

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                return "Invalid email format";

            if (!update)
            {
                var existingEmail = await _userRepository.GetUserByEmail(email);
                if (existingEmail != null)
                    return "Email already exists";
            }

            return null;
        }

        private async Task<string?> IsValidUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return "Username are required";

            userName = userName.Trim();

            if (userName.Any(char.IsWhiteSpace))
                return "Username must not contain spaces";

            string pattern = @"^[a-zA-Z0-9]+$";
            if (!Regex.IsMatch(userName, pattern))
                return "Username must be alphanumeric without spaces or special characters";

            var existingUser = await _userRepository.GetUserByUsername(userName);
            if (existingUser != null)
                return "Username already exists";

            return null;
        }
    }
}
