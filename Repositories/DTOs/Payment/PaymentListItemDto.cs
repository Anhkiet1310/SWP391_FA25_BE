using Repositories.Enum;

namespace Repositories.DTOs.Payment
{
    public class PaymentListItemDto
    {
        public int PaymentId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public string? CarName { get; set; }
        public string? PlateNumber { get; set; }
        public string? OrderId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountVnd { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
