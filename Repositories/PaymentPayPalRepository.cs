using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Repositories.DTOs.Payment;

namespace Repositories
{
    public class PaymentPayPalRepository
    {
        private readonly PayPalEnvironment _environment;
        private readonly PayPalHttpClient _client;

        public PaymentPayPalRepository(string clientId, string clientSecret, bool isSandbox = true)
        {
            _environment = isSandbox
                ? new SandboxEnvironment(clientId, clientSecret)
                : new LiveEnvironment(clientId, clientSecret);
            _client = new PayPalHttpClient(_environment);
        }

        public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto paymentRequest)
        {
            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = paymentRequest.Currency,
                            Value = paymentRequest.Amount.ToString("F2")
                        },
                        Description = paymentRequest.Description
                    }
                },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = "https://example.com/return",
                    CancelUrl = "https://example.com/cancel"
                }
            };

            var requestObj = new OrdersCreateRequest();
            requestObj.Prefer("return=representation");
            requestObj.RequestBody(orderRequest);

            var response = await _client.Execute(requestObj);
            var result = response.Result<Order>();

            var approvalUrl = result.Links.FirstOrDefault(link => link.Rel == "approve")?.Href;

            return new PaymentResponseDto
            {
                Status = result.Status,
                ApprovalUrl = approvalUrl ?? "",
                OrderId = result.Id
            };
        }

        public async Task<string> CapturePayment(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());
            var response = await _client.Execute(request);
            var result = response.Result<Order>();
            return result.Status;
        }
    }
}
