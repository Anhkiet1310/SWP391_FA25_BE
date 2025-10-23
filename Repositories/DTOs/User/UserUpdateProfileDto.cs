using Repositories.Entities;

namespace Repositories.DTOs.User
{
    public class UserUpdateProfileDto
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? CCCDFront { get; set; }
        public string? CCCDBack { get; set; }
    }
}
