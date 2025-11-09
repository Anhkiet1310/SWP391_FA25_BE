using PayOS;
using PayOS.Exceptions;
using PayOS.Models.V2.PaymentRequests;
using Repositories.DTOs.Payment;

namespace Repositories
{
    public class PaymentPayOSRepository
    {
        private readonly PayOSClient _client;

        public PaymentPayOSRepository(string clientId, string apiKey, string checksumKey)
        {
            _client = new PayOSClient(clientId, apiKey, checksumKey);
        }

        public async Task<PaymentResponseDto> CreatePayment(PaymentPayOSRequestDto payment)
        {
            try
            {
                long expireAt = (long)DateTime.UtcNow.AddMinutes(5).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                var paymentRequest = new CreatePaymentLinkRequest
                {
                    OrderCode = payment.OrderId,
                    Amount = payment.Amount,
                    Description = payment.Description,
                    ReturnUrl = payment.ReturnUrl,
                    CancelUrl = payment.CancelUrl,
                    ExpiredAt = expireAt
                };
                var paymentLink = await _client.PaymentRequests.CreateAsync(paymentRequest);
                return new PaymentResponseDto
                {
                    OrderId = paymentLink.PaymentLinkId,
                    Status = paymentLink.Status.ToString(),
                    ApprovalUrl = paymentLink.CheckoutUrl
                };
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
                Console.WriteLine($"Error Code: {ex.ErrorCode}");
                return null;
            }
            catch (PayOSException ex)
            {
                Console.WriteLine($"PayOS Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string> CapturePayment(string orderId)
        {
            var response = await _client.PaymentRequests.GetAsync(orderId);
            return response.Status.ToString();
        }
    }
}
