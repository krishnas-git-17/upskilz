using Microsoft.AspNetCore.Mvc;
using theupskilzapi.DTOs;
using theupskilzapi.Services;

namespace theupskilzapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;

        public PaymentController(RazorpayService razorpayService)
        {
            _razorpayService = razorpayService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] PaymentRequestDto dto)
        {
            var response = await _razorpayService.CreateOrderAsync(dto);
            return Ok(response);
        }
    }
}
