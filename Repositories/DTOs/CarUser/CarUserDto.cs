using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.CarUser
{
    public class CarUserDto
    {
        public int CarUserId { get; set; }
        public int CarId { get; set; }
        public string? CarName { get; set; }
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public double? OwnershipPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
