using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class User : BasicElement
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? CCCDFront { get; set; }
        public string? CCCDBack { get; set; }
        public int Role { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<CarUser> CarUsers { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
