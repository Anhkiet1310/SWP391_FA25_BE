using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Schedule : BasicElement
    {
        public int ScheduleId { get; set; }
        public int CarUserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public CarUser CarUser { get; set; } 
    }
}
