using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.PaymentDTO;
using ServanaAPP.Interfaces;

namespace ServanaAPP.Controllers
{
    
    
    [Route("api/[controller]")]
    [ApiController]

    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("pay-job")]
        public async Task<IActionResult> PayJob([FromBody] PayDTO input)
        {
            try
            {
                var result = await _paymentService.HandlePaymentAsync(input.RequestID, input.Method);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
