using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Maintenance
{
    public class MaintenanceCreateDto
    {
        public int CarId { get; set; }
        public string MaintenanceType { get; set; }
        public DateTime MaintenanceDay { get; set; }
        public int Status { get; set; }  // Trạng thái bảo dưỡng dưới dạng int (1, 2, 3, 4)
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
