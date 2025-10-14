using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.Payment;
using Services;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService, IConfiguration config)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment(PaymentRequestDto paymentRequest)
        {
            if (paymentRequest == null || paymentRequest.Amount <= 0 || string.IsNullOrEmpty(paymentRequest.Currency))
            {
                return BadRequest("Invalid payment request");
            }
            try
            {
                var paymentResponse = await _paymentService.CreatePayment(paymentRequest);
                return Ok(paymentResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("capture/{orderId}")]
        public async Task<IActionResult> CapturePayment(string orderId, int userId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest("Invalid order ID");
            }
            try
            {
                var captureResult = await _paymentService.CapturePayment(orderId, userId);
                return Ok(new {message = "Payment successful!", Status = captureResult });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
