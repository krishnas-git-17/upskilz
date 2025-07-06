using Microsoft.AspNetCore.Mvc;
using theupskilzapi.DTOs;
using theupskilzapi.Services;

namespace theupskilzapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;

        public PaymentController(RazorpayService razorpayService)
        {
            _razorpayService = razorpayService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] PaymentRequestDto request)
        {
            try
            {
                var result = await _razorpayService.CreateOrderAsync(request);
                return Ok(new { success = true, order = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
