using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class CarUser : BasicElement
    {
        public int CarUserId { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        public Car Car { get; set; } 
        public User User { get; set; }
        public PercentOwnership PercentOwnership { get; set; }
        public ICollection<Schedule> Schedules { get; set; } 
        public ICollection<Contract> Contracts { get; set; } 
        public ICollection<Transaction> Transactions { get; set; }
    }
}
