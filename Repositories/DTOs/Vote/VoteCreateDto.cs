using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Vote
{
    public class VoteCreateDto
    {
        public int UserId { get; set; }
        public int FormId { get; set; }
        public bool Decision { get; set; } // true = Đồng ý, false = Không đồng ý
    }
}
