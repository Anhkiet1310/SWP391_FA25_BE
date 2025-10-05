using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Group
{
    public class GroupCreateDto
    {
        public int CarId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupImg { get; set; }  // đường dẫn ảnh nhóm (Cloudflare, Firebase...)
    }
}
