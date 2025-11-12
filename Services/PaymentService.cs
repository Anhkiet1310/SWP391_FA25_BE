using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DTOs.Payment;
using Repositories.Entities;
using Repositories.Enum;
using Services.Utils;
using System.Numerics;
using System.Text.Json;

namespace Services
{
    public class PaymentService
    {
        private readonly PaymentPayPalRepository _paymentPayPalRepository;
        private readonly PaymentPayOSRepository _paymentPayOSRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly CarUserRepository _carUserRepository;
        private readonly CarRepository _carRepository;
        private readonly UserRepository _userRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;


        public PaymentService(PaymentPayPalRepository paymentPayPalRepository, PaymentPayOSRepository paymentPayOSRepository, PaymentRepository paymentRepository, CarUserRepository carUserRepository, CarRepository carRepository, UserRepository userRepository, TransactionRepository transactionRepository, IMapper mapper, HttpClient httpClient)
        {
            _paymentPayPalRepository = paymentPayPalRepository;
            _paymentPayOSRepository = paymentPayOSRepository;
            _paymentRepository = paymentRepository;
            _carUserRepository = carUserRepository;
            _carRepository = carRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<ServiceResult<PaymentResponseDto>> TestPayOSAsync()
        {
            long randomOrderId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
            var mockData = new PaymentPayOSRequestDto()
            {
                OrderId = 123456789,
                Amount = 10000,
                CancelUrl = "https://your-site.com/cancel",
                Description = "Payment for order #123",
                ReturnUrl = "https://your-site.com/success",
            };
            var paymentResponse = _paymentPayOSRepository.CreatePayment(mockData);
            return new ServiceResult<PaymentResponseDto>
            {
                Success = true,
                Message = "Payment created successfully",
                Data = new PaymentResponseDto
                {
                    ApprovalUrl = paymentResponse.Result.ApprovalUrl,
                    Status = paymentResponse.Result.Status,
                    OrderId = paymentResponse.Result.OrderId
                }
            };
        }

        public async Task<List<PaymentListItemDto>> GetAllPayment()
        {
            var exchangeRate = await GetUsdToVndRate();
            var query = _paymentRepository.GetAllPaymentQuery();
            //var payments = await query.ProjectTo<PaymentListItemDto>(_mapper.ConfigurationProvider)
            //                          .ToListAsync();
            var payments = await query.ToListAsync();

            var transaction = await _transactionRepository.GetTransactionByOrderId(payments.First().OrderId);
            var carUser = await _carUserRepository.GetCarUserById(transaction.CarUserId);
            var car = await _carRepository.GetByIdAsync(carUser.CarId);

            var result = payments.Select(p => new PaymentListItemDto
            {
                PaymentId = p.PaymentId,
                CarName = car.CarName,
                PlateNumber = car.PlateNumber,
                OrderId = p.OrderId,
                Amount = p.Amount,
                Currency = p.Currency,
                AmountVnd = p.Amount * exchangeRate,
                Description = p.Description,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status.ToString(),
                CreatedAt = p.CreatedAt,
            }).ToList();

            return result;

            //foreach (var payment in payments)
            //{
            //    if (payment.Currency == "USD")
            //    {
            //        payment.AmountVnd = payment.Amount * exchangeRate;
            //    }
            //}
            //return payments;
        }

        public async Task<ServiceResult<PaymentResponseDto>> CreatePayment(PaymentRequestDto paymentRequest)
        {
            var payPalResponse = await _paymentPayPalRepository.CreatePayment(paymentRequest);
            //var carUser = await _carUserRepository.GetCarUserByUserId(paymentRequest.UserId, carId);
            //if (carUser == null)
            //    return new ServiceResult<PaymentResponseDto>
            //    {
            //        Success = false,
            //        Message = "Caruser not found."
            //    };
            var user = await _userRepository.GetUserById(paymentRequest.UserId);
            if (user == null)
            {
                return new ServiceResult<PaymentResponseDto>
                {
                    Success = false,
                    Message = "User with id not found."
                };
            }

            var payment = new Payment
            {
                UserId = paymentRequest.UserId,
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
                CarUserId = null,
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

        public async Task<ServiceResult<PaymentResponseDto>> CreatePaymentWithPayOS(PaymentRequestDto paymentRequest)
        {
            var value = new BigInteger(Guid.NewGuid().ToByteArray().Concat(new byte[] { 0 }).ToArray());
            long randomOrderId = (long)(BigInteger.Abs(value) % 10000000000L);
            var payOSRequest = new PaymentPayOSRequestDto()
            {
                OrderId = randomOrderId,
                Amount = (long)paymentRequest.Amount,
                ReturnUrl = paymentRequest.ReturnUrl,
                CancelUrl = paymentRequest.CancelUrl,
            };
            var payOSResponse = await _paymentPayOSRepository.CreatePayment(payOSRequest);
            //var carUser = await _carUserRepository.GetCarUserByUserId(paymentRequest.UserId, carId);
            //if (carUser == null)
            //    return new ServiceResult<PaymentResponseDto>
            //    {
            //        Success = false,
            //        Message = "Car user not found."
            //    };
            var user = await _userRepository.GetUserById(paymentRequest.UserId);
            if (user == null)
            {
                return new ServiceResult<PaymentResponseDto>
                {
                    Success = false,
                    Message = "User with id not found."
                };
            }

            var payment = new Payment
            {
                UserId = paymentRequest.UserId,
                PaymentMethod = "PayOS",
                Status = StatusPayment.Pending,
                OrderId = payOSResponse.OrderId,
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                Description = paymentRequest.Description,
                CreatedAt = DateTime.UtcNow
            };
            await _paymentRepository.AddPayment(payment);

            var transaction = new Transaction
            {
                CarUserId = null,
                PaymentId = payment.PaymentId,
                Amount = paymentRequest.Amount,
                TransactionType = paymentRequest.TransactionType,
                Status = Status.Pending,
                OrderId = payOSResponse.OrderId,
                CreatedAt = DateTime.UtcNow
            };
            await _transactionRepository.AddTransaction(transaction);

            return new ServiceResult<PaymentResponseDto>
            {
                Success = true,
                Data = payOSResponse
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

        public async Task<ServiceResult<string>> CapturePayOSPayment(string orderId, int userId)
        {
            var status = await _paymentPayOSRepository.CapturePayment(orderId);
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

            if (status.ToLower() == "paid")
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
            else if (status.ToLower() == "cancelled")
            {
                transaction.Status = Status.Failed;
                payment.Status = StatusPayment.Canceled;
                await _paymentRepository.UpdatePayment(payment);
                await _transactionRepository.UpdateTransaction(transaction);
            }
            return new ServiceResult<string>
            {
                Success = true,
                Data = status
            };
        }

        public async Task<ServiceResult<bool>> PayWithWallet(PaymentWalletRequestDto paymentWalletRequestDto, int carId)
        {
            var carUser = await _carUserRepository.GetCarUserByUserId(paymentWalletRequestDto.UserId, carId);
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
                    UserId = paymentWalletRequestDto.UserId,
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

        private async Task<decimal> GetUsdToVndRate()
        {
            var response = await _httpClient.GetStringAsync("https://open.er-api.com/v6/latest/USD");
            var jsonDoc = JsonDocument.Parse(response);
            var rates = jsonDoc.RootElement.GetProperty("rates");
            return rates.GetProperty("VND").GetDecimal();
        }
    }
}
