using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public string Status { get; set; }
        public string ApprovalUrl { get; set; }
        public string OrderId { get; set; }
    }
}
