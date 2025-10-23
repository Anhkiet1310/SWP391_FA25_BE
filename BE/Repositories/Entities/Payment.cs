using Repositories.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Payment : BasicElement
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string OrderId { get; set; }         
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
