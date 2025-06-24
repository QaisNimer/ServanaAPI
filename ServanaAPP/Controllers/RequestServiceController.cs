using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ServanaAPP.DTOs.AcceptorReject;
using ServanaAPP.DTOs.RequestService.Request;
using ServanaAPP.DTOs.RequestService.Response;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestServiceController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly ServanaDbContext _db;

        public RequestServiceController(IRequestService requestService, ServanaDbContext db) {

            _requestService = requestService;
            _db = db;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RequestNewService([FromBody] RequestServiceDTO input)
        {


            try
            {
                var exists = await _db.JobRequests.AnyAsync(j =>
                                j.ClientID == input.clientID &&
                                j.WorkerID == input.workerID &&
                                j.IsActive &&
                                (j.Status.ToLower() != "completed" && j.Status.ToLower() != "paid"));


                if (exists)
                    throw new Exception("You already have an ongoing request with this worker.");
                var result = await _requestService.RequestServicee(input);
                var response = new ResponseServiceDTO
                {
                    RequestID = result.RequestID,
                    ClientID = result.ClientID,
                    WorkerID = result.WorkerID,
                    Description = result.Description,
                    Status = result.Status,
                    PaymentMethod = result.PaymentMethod,
                    CreatedAt = result.CreatedAt,
                    CreatedBy = result.CreatedBy
                };

                return Ok(new
                {
                    message = "Job request created successfully.",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {

                    error = ex.Message
                });
            }
        }
        [HttpPost("AcceptOrRejectRequest")]
        public async Task<IActionResult> AcceptOrRejectRequest([FromBody] AcceptorRejectRequestDTO input)
        {
            try
            {
                var result = await _requestService.AcceptOrRejectRequestAsync(input);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    } 


    
}

