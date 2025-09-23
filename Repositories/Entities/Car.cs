using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Car : BasicElement
    {
        public int CarId { get; set; }
        public string? Brand { get; set; }   
        public string? CarName { get; set; }
        public string? PlateNumber { get; set; }
        public int Status { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public int BatteryCapacity { get; set; }
        public Group Group { get; set; }
        public ICollection<CarUser> CarUsers { get; set; }
        public ICollection<CarDetail> CarDetails { get; set; }
    }
}
