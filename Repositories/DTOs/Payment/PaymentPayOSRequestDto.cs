using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Payment
{
    public class PaymentPayOSRequestDto
    {
        public long Amount { get; set; }
        public long OrderId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
