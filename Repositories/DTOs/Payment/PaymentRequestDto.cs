using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}
