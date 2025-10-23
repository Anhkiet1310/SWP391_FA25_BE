using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Group : BasicElement
    {
        public int GroupId { get; set; }
        public int CarId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupImg { get; set; }
        public Car Car { get; set; }
        public ICollection<Form> Forms { get; set; } 
        public ICollection<UserGroup> UserGroups { get; set; } 
    }
}
