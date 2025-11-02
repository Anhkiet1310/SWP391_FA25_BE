using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DTOs.Payment;
using Repositories.Entities;
using Repositories.Enum;
using Services.Utils;

namespace Services
{
    public class PaymentService
    {
        private readonly PaymentPayPalRepository _paymentPayPalRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly CarUserRepository _carUserRepository;
        private readonly UserRepository _userRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public PaymentService(PaymentPayPalRepository paymentPayPalRepository, PaymentRepository paymentRepository, CarUserRepository carUserRepository, UserRepository userRepository, TransactionRepository transactionRepository, IMapper mapper)
        {
            _paymentPayPalRepository = paymentPayPalRepository;
            _paymentRepository = paymentRepository;
            _carUserRepository = carUserRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<List<PaymentListItemDto>> GetAllPayment()
        {
            var query = _paymentRepository.GetAllPaymentQuery();
            var payments = await query.ProjectTo<PaymentListItemDto>(_mapper.ConfigurationProvider)
                                      .ToListAsync();

            return payments;
        }

        public async Task<ServiceResult<PaymentResponseDto>> CreatePayment(PaymentRequestDto paymentRequest)
        {
            var payPalResponse = await _paymentPayPalRepository.CreatePayment(paymentRequest);
            var carUser = await _carUserRepository.GetCarUserByUserId(paymentRequest.UserId);
            if (carUser == null)
                return new ServiceResult<PaymentResponseDto>
                {
                    Success = false,
                    Message = "Caruser not found."
                };

            var payment = new Payment
            {
                CarUserId = carUser.CarUserId,
                PaymentMethod = "PayPal",
                Status = StatusPayment.Pending,
                OrderId = payPalResponse.OrderId,
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                Description = paymentRequest.Description,
                CreatedAt = DateTime.UtcNow
            };
            await _paymentRepository.AddPayment(payment);

            var transaction = new Transaction
            {
                CarUserId = carUser.CarUserId,
                Amount = paymentRequest.Amount,
                TransactionType = paymentRequest.TransactionType,
                Status = Status.Pending,
                OrderId = payPalResponse.OrderId,
                CreatedAt = DateTime.UtcNow
            };
            await _transactionRepository.AddTransaction(transaction);

            return new ServiceResult<PaymentResponseDto>
            {
                Success = true,
                Data = payPalResponse
            };
        }

        public async Task<ServiceResult<string>> CapturePayment(string orderId, int userId)
        {
            var status = await _paymentPayPalRepository.CapturePayment(orderId);
            var transaction = await _transactionRepository.GetTransactionByOrderId(orderId);
            var payment = await _paymentRepository.GetPaymentByOrderId(orderId);

            if (transaction == null)
                return new ServiceResult<string>
                {
                    Success = false,
                    Message = "Transaction not found."
                };

            if (payment == null)
                return new ServiceResult<string>
                {
                    Success = false,
                    Message = "Payment not found."
                };

            if (status == "COMPLETED")
            {
                transaction.Status = Status.Completed;
                payment.Status = StatusPayment.Paided;
                await _transactionRepository.UpdateTransaction(transaction);
                await _paymentRepository.UpdatePayment(payment);

                if (transaction.TransactionType == TransactionType.Deposit)
                {
                    var user = await _userRepository.GetUserById(userId);
                    user.Balance += transaction.Amount;
                    await _userRepository.UpdateProfile(user);
                }
            }
            else
            {
                transaction.Status = Status.Failed;
                await _transactionRepository.UpdateTransaction(transaction);
            }
            return new ServiceResult<string>
            {
                Success = true,
                Data = status
            };
        }

        public async Task<ServiceResult<bool>> PayWithWallet(PaymentWalletRequestDto paymentWalletRequestDto)
        {
            var carUser = await _carUserRepository.GetCarUserByUserId(paymentWalletRequestDto.UserId);
            if (carUser == null)
                return new ServiceResult<bool>
                {
                    Success = false,
                    Message = "CarUser not found."
                };

            var user = await _userRepository.GetUserById(carUser.UserId);
            if (user.Balance < paymentWalletRequestDto.Amount)
                return new ServiceResult<bool>
                {
                    Success = false,
                    Message = "Insufficient balance."
                };

            try
            {
                var orderId = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}-{paymentWalletRequestDto.UserId}";
                var payment = new Payment
                {
                    PaymentMethod = "Wallet",
                    Status = StatusPayment.Paided,
                    OrderId = orderId,
                    CarUserId = carUser.CarUserId,
                    Amount = paymentWalletRequestDto.Amount,
                    Currency = "USD",
                    Description = paymentWalletRequestDto.Description,
                    CreatedAt = DateTime.UtcNow
                };
                await _paymentRepository.AddPayment(payment);

                var transaction = new Transaction
                {
                    OrderId = orderId,
                    PaymentId = payment.PaymentId,
                    CarUserId = carUser.CarUserId,
                    Amount = paymentWalletRequestDto.Amount,
                    TransactionType = TransactionType.Purchase,
                    Status = Status.Completed,
                    CreatedAt = DateTime.UtcNow
                };
                await _transactionRepository.AddTransaction(transaction);

                user.Balance -= paymentWalletRequestDto.Amount;
                await _userRepository.UpdateProfile(user);

                return new ServiceResult<bool>
                {
                    Success = true,
                    Message = "Payment successful using wallet.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                user.Balance += paymentWalletRequestDto.Amount;
                await _userRepository.UpdateProfile(user);

                return new ServiceResult<bool>
                {
                    Success = false,
                    Message = $"Payment failed: {ex.Message}"
                };
            }
        }
    }
}
