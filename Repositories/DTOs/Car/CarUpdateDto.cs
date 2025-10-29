using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Car
{
    public class CarUpdateDto
    {
        public string? Brand { get; set; }
        public string? CarName { get; set; }
        public string? PlateNumber { get; set; }
        public int Status { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public int BatteryCapacity { get; set; }
    }
}
