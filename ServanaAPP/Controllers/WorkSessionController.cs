using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.EndWork;
using ServanaAPP.DTOs.StartWork;
using ServanaAPP.Helpers.Firebase;
using ServanaAPP.Interfaces;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkSessionController : ControllerBase
    {
        private readonly IWorkSession _service;
        private readonly SendNotificationHelper _sendNotificationHelper;
        public WorkSessionController(IWorkSession service)
        {
            _service = service;
        }

        [HttpPost("StartWork")]
        public async Task<IActionResult> StartWork([FromBody] StartWorkDTO input)
        {
            try
            {
                var result = await _service.StartWorkAsync(input);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("EndWork")]
        public async Task<IActionResult> EndWork([FromBody] EndWorkDTO input)
        {
            try
            {
                var result = await _service.EndWorkAsync(input);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
