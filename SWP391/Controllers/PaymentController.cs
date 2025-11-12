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

        [HttpGet("/test")]
        public IActionResult GetUrl()
        {
            var response = _paymentService.TestPayOSAsync().Result;
            return Ok(new
            {
                success = true,
                data = response.Data
            });
        }

        [HttpGet()]
        public IActionResult GetAllPayment()
        {
            try
            {
                var payments = _paymentService.GetAllPayment().Result;
                return Ok(new
                {
                    success = true,
                    data = payments
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> CreateDeposit(PaymentRequestDto paymentRequest)
        {
            if (paymentRequest == null || paymentRequest.Amount <= 0 || string.IsNullOrEmpty(paymentRequest.Currency))
            {
                return BadRequest("Invalid payment request");
            }
            try
            {
                var paymentResponse = await _paymentService.CreatePayment(paymentRequest);
                if (paymentResponse.Success == false)
                {
                    return BadRequest(new
                    {
                        success = paymentResponse.Success,
                        message = paymentResponse.Message
                    });
                }
                return Ok(new
                {
                    success = paymentResponse.Success,
                    data = paymentResponse.Data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("paypal")]
        public async Task<IActionResult> CreatePaypal(PaymentRequestDto paymentRequest)
        {
            if (paymentRequest == null || paymentRequest.Amount <= 0 || string.IsNullOrEmpty(paymentRequest.Currency))
            {
                return BadRequest("Invalid payment request");
            }
            try
            {
                var paymentResponse = await _paymentService.CreatePayment(paymentRequest);
                if (paymentResponse.Success == false)
                {
                    return BadRequest(new
                    {
                        success = paymentResponse.Success,
                        message = paymentResponse.Message
                    });
                }
                return Ok(new
                {
                    success = paymentResponse.Success,
                    data = paymentResponse.Data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("payos")]
        public async Task<IActionResult> CreatePayOS(PaymentRequestDto paymentRequest)
        {
            if (paymentRequest == null || paymentRequest.Amount <= 0 || string.IsNullOrEmpty(paymentRequest.Currency))
            {
                return BadRequest("Invalid payment request");
            }
            try
            {
                var paymentResponse = await _paymentService.CreatePaymentWithPayOS(paymentRequest);
                if (paymentResponse.Success == false)
                {
                    return BadRequest(new
                    {
                        success = paymentResponse.Success,
                        message = paymentResponse.Message
                    });
                }
                return Ok(new
                {
                    success = paymentResponse.Success,
                    data = paymentResponse.Data
                });
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
                if (captureResult.Success == false)
                    return BadRequest(new
                    {
                        success = captureResult.Success,
                        message = captureResult.Message
                    });
                return Ok(new 
                {
                    success = captureResult.Success,
                    message = "Payment successful!", 
                    Status = captureResult.Data 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("capture/payos/{orderId}")]
        public async Task<IActionResult> CapturePayOSPayment(string orderId, int userId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest("Invalid order ID");
            }
            try
            {
                var captureResult = await _paymentService.CapturePayOSPayment(orderId, userId);
                if (captureResult.Success == false)
                    return BadRequest(new
                    {
                        success = captureResult.Success,
                        message = captureResult.Message
                    });
                return Ok(new
                {
                    success = captureResult.Success,
                    message = "Payment successful!",
                    Status = captureResult.Data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("wallet")]
        public async Task<IActionResult> PayFromWallet(PaymentWalletRequestDto paymentRequest, int carId)
        {
            if (paymentRequest == null || paymentRequest.Amount <= 0)
            {
                return BadRequest("Invalid payment request");
            }
            try
            {
                var paymentResponse = await _paymentService.PayWithWallet(paymentRequest, carId);
                if (paymentResponse.Success == false)
                {
                    return BadRequest(new
                    {
                        success = paymentResponse.Success,
                        message = paymentResponse.Message
                    });
                }
                return Ok(new 
                { 
                    success = paymentResponse.Success,
                    data = paymentResponse.Data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
