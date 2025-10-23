using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Vote : BasicElement
    {
        public int VoteId { get; set; }
        public int UserId { get; set; }
        public int FormId { get; set; }
        public bool Decision { get; set; }
        public Form Form { get; set; }
        public User User { get; set; }
    }
}
