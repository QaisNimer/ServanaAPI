using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Helpers.Firebase;
using ServanaAPP.Interfaces;
using ServanaAPP.Services;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendNotificationController : ControllerBase
    {
        private readonly SendNotificationHelper _sendNotificationHelper;
        private readonly IUpdateDeviceToken _updateDeviceToken;
        public SendNotificationController(SendNotificationHelper sendNotificationHelper, IUpdateDeviceToken updateDeviceToken) 
        {
        _sendNotificationHelper= sendNotificationHelper;
            _updateDeviceToken = updateDeviceToken;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateDeviceToken([FromBody] UpdateDeviceTokenRequestDTO input)
        {
            try
            {
                var UpdateDeviceToken = await _updateDeviceToken.UpdateDeviceToken(input);
                return Ok(UpdateDeviceToken);
            }
            catch (Exception ex)
            {

                return StatusCode(500,ex.Message);
            }
        }
    }

}
