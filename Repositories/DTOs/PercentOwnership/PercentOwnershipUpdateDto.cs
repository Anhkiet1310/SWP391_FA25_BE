using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.PercentOwnership
{
    public class PercentOwnershipUpdateDto
    {
        public double? Percentage { get; set; }
        public double? UsageLimit { get; set; }
    }
}
