using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class CarDetail : BasicElement
    {
        public int CarDetailId { get; set; }
        public int CarId { get; set; }
        public string? Original { get; set; }
        public string? CheckIn { get; set; }
        public string? CheckOut { get; set; }
        public string? BatteryCapacityIn { get; set; }
        public string? BatteryCapacityOut { get; set; }
        public Car Car { get; set; }
    }
}
