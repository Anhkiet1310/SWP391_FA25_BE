using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Contract : BasicElement
    {
        public int ContractId { get; set; }
        public int CarUserId { get; set; }
        public string? ContractTitle { get; set; } 
        public string? ContractTerms { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CarUser CarUser { get; set; } 
    }
}
