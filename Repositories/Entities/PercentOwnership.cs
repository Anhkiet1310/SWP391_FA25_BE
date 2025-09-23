using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class PercentOwnership : BasicElement
    {
        public int PercentOwnershipId { get; set; }
        public int CarUserId { get; set; }
        public double Percentage { get; set; }
        public double UsageLimit { get; set; }
        public CarUser CarUser { get; set; }
    }
}
