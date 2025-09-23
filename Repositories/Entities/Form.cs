using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Form : BasicElement
    {
        public int FormId { get; set; }
        public int GroupId { get; set; }
        public string FormTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Group Group { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
