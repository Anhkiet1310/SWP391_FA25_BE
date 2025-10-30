using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.PercentOwnership
{
    public class PercentOwnershipCreateDto
    {
        public int CarUserId { get; set; }
        public double Percentage { get; set; }
        public double UsageLimit { get; set; }
    }
}
