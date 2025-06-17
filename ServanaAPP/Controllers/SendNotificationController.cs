using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Helpers.Firebase;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendNotificationController : ControllerBase
    {
        private readonly SendNotificationHelper _sendNotificationHelper;
        public SendNotificationController(SendNotificationHelper sendNotificationHelper) 
        {
        _sendNotificationHelper= sendNotificationHelper;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequestDTO input)
        {
            try
            {
                var result = await _sendNotificationHelper.SendNotificationAsync(input);
                return Ok(new { MessageId = result });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
