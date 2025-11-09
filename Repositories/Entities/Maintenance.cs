using Repositories.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Maintenance : BasicElement
    {
        public int MaintenanceId { get; set; }
        public int CarId { get; set; }
        public string? MaintenanceType { get; set; }
        public DateTime MaintenanceDay { get; set; }
        public MaintenanceStatus Status { get; set; }  
        public string? Description { get; set; }
        public decimal Price { get; set; }

        // Quan hệ
        public Car Car { get; set; }
    }
}
