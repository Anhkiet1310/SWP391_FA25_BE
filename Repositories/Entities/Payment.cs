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
        public int TransactionId { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public Transaction Transaction { get; set; }
    }
}
