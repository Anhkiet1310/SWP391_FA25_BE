using Repositories.Entities;

namespace Repositories.DTOs.User
{
    public class UserResponseDto : BasicElement
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public decimal Balance { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? CCCDFront { get; set; }
        public string? CCCDBack { get; set; }
        public int Role { get; set; }
    }
}
