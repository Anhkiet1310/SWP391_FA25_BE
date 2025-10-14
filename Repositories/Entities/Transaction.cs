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
        public int PaymentId { get; set; }
        public int CarUserId { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }
        public Payment Payment { get; set; }
        public CarUser CarUser { get; set; }
    }
}
