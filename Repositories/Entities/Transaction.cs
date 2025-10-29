using Repositories.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Transaction : BasicElement
    {
        public int TransactionId { get; set; }
        public int? PaymentId { get; set; }
        public int CarUserId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public Status Status { get; set; }
        public string OrderId { get; set; }
        public Payment Payment { get; set; }
        public CarUser CarUser { get; set; }
    }
}
