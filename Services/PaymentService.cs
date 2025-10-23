using Repositories;
using Repositories.DTOs.Payment;
using Repositories.Entities;
using Repositories.Enum;

namespace Services
{
    public class PaymentService
    {
        private readonly PaymentPayPalRepository _paymentPayPalRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly CarUserRepository _carUserRepository;

        public PaymentService(PaymentPayPalRepository paymentPayPalRepository, PaymentRepository paymentRepository, CarUserRepository carUserRepository)
        {
            _paymentPayPalRepository = paymentPayPalRepository;
            _paymentRepository = paymentRepository;
            _carUserRepository = carUserRepository;
        }

        public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto paymentRequest)
        {
            var payment = await _paymentPayPalRepository.CreatePayment(paymentRequest);

            var newPayment = new Payment
            {
                PaymentMethod = "PayPal",
                Status = PaymentStatus.Pending,
                OrderId = payment.OrderId,
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                Description = paymentRequest.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepository.AddPayment(newPayment);
            return payment;
        }

        public async Task<string> CapturePayment(string orderId, int userId)
        {
            var status = await _paymentPayPalRepository.CapturePayment(orderId);
            var payment = await _paymentRepository.GetPaymentByOrderId(orderId);
            var carUser = await _carUserRepository.GetCarUserByUserId(userId);

            if (payment == null)
                throw new Exception("Payment not found");

            if (status == "COMPLETED")
            {
                payment.Status = PaymentStatus.Completed;
                await _paymentRepository.UpdatePayment(payment);

                var transaction = new Transaction
                {
                    PaymentId = payment.PaymentId,
                    CarUserId = carUser.CarUserId,
                    Amount = payment.Amount,
                    TransactionType = "Balance",
                    CreatedAt = DateTime.UtcNow
                };
                await _paymentRepository.AddTransaction(transaction);
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                await _paymentRepository.UpdatePayment(payment);
            }
            return status;
        }
    }
}
