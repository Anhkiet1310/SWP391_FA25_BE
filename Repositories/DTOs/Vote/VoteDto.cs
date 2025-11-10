using Repositories.DTOs.Form;
using Repositories.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Vote
{
    public class VoteDto
    {
        public int VoteId { get; set; }
        public int UserId { get; set; }
        public int FormId { get; set; }
        public bool Decision { get; set; }

        // Thông tin User
        public UserResponseDto User { get; set; }

        // Thông tin Form
        public FormCreateDto Form { get; set; }
    }

}
