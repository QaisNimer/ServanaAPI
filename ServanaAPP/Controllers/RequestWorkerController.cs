using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.RequestWorker.Request;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Helpers.Firebase;
using ServanaAPP.Models;
using ServanaAPP.Services;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestWorkerController : ControllerBase
    {
        private readonly RequestWorkerService _requestWorkerService;
        private readonly SendNotificationHelper _sendNotificationHelper;

        public RequestWorkerController(RequestWorkerService requestWorkerService, SendNotificationHelper sendNotificationHelper) 
        {
            _requestWorkerService=requestWorkerService;
            _sendNotificationHelper=sendNotificationHelper;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> RequestWorker(RequestWorkerDTOs input) {
            try
            {
                var RequestWorker = await _requestWorkerService.RequestServiceWorker(input);
                SendNotificationRequestDTO sendNotification = new SendNotificationRequestDTO { 
                    Title = input.Title,
                    Body = input.Body,
                    DeviceToken= input.DeviceToken
                };
                await _sendNotificationHelper.SendNotificationAsync(sendNotification);

                return StatusCode(200, RequestWorker);
            }
            catch (Exception ex)
            {

                return StatusCode(500,ex.Message);
            }
        }
    }
}
