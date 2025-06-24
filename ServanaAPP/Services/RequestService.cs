using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServanaAPP.DTOs.AcceptorReject;
using ServanaAPP.DTOs.RequestService.Request;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Helpers.Firebase;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Services
{
    public class RequestService : IRequestService
    {
        private readonly ServanaDbContext _db;
        private readonly SendNotificationHelper _sendNotificationHelper;
        public RequestService (ServanaDbContext db, SendNotificationHelper sendNotificationHelper)
        {
            _db = db;
            _sendNotificationHelper = sendNotificationHelper;
        }

        public async Task<string> AcceptOrRejectRequestAsync(AcceptorRejectRequestDTO input)
        {
            var jobRequest = await _db.JobRequests.FindAsync(input.RequestID);
            if (jobRequest == null || !jobRequest.IsActive)
                throw new Exception("Job request not found or inactive.");

            if (jobRequest.Status.ToLower() != "pending")
                throw new Exception("Only pending requests can be updated.");

            jobRequest.Status = input.IsAccepted ? "accepted" : "rejected";
            jobRequest.UpdatedAt = DateTime.Now;
            jobRequest.UpdatedBy = "system";
            await _db.SaveChangesAsync();

            if (input.IsAccepted)
            {
                var existingSession = await _db.WorkSessions
                    .FirstOrDefaultAsync(ws => ws.RequestID == jobRequest.RequestID);

                if (existingSession == null)
                {
                    var workSession = new WorkSession
                    {
                        RequestID = jobRequest.RequestID,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System"
                    };

                    _db.WorkSessions.Add(workSession);
                    await _db.SaveChangesAsync(); 
                }
            }

            var client = await _db.Users
            .Where(u => u.UserID == jobRequest.ClientID && u.IsActive)
            .Select(u => new { u.FullName, u.DeviceToken })
            .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(client?.DeviceToken))
            {
                string title = input.IsAccepted ? "Request Accepted ✅" : "Request Rejected ❌";
                string body = input.IsAccepted
                    ? $"Your service request was accepted by worker."
                    : $"Your service request was rejected by worker.";

                await _sendNotificationHelper.SendNotificationAsync(new SendNotificationRequestDTO
                {
                    Title = title,
                    Body = body,
                    DeviceToken = client.DeviceToken
                });
            }

            return input.IsAccepted ? "Request accepted successfully." : "Request rejected successfully.";

        }

        public async Task<JobRequest> RequestServicee(RequestServiceDTO input)
        {
            try
            {



                JobRequest job = new JobRequest();
                job.ClientID = input.clientID;
                job.WorkerID = input.workerID;
                job.Description = input.description;
                job.Status = input.status;
                job.CreatedAt = DateTime.Now;
                job.CreatedBy = "system";
                job.IsActive = true;
                _db.JobRequests.Add(job);
                await _db.SaveChangesAsync();

                //Fetch client and worker tokens
                var clientUser = await _db.Users
                    .Where(u => u.UserID == job.ClientID)
                    .Select(u => new { u.FullName, u.DeviceToken })
                    .FirstOrDefaultAsync();


                var workerUser = await _db.Users
                    .Where(u => u.UserID == job.WorkerID)
                    .Select(u => new { u.FullName, u.DeviceToken })
                    .FirstOrDefaultAsync();

                // Send to Client
                if (!string.IsNullOrEmpty(clientUser?.DeviceToken))
                {
                    await _sendNotificationHelper.SendNotificationAsync(new SendNotificationRequestDTO
                    {
                        Title = "Request Submitted",
                        Body = $"Your request to {workerUser?.FullName} is pending confirmation.",
                        DeviceToken = clientUser.DeviceToken
                    });
                }

                // send to worker
                if (!string.IsNullOrEmpty(workerUser?.DeviceToken))
                {
                    await _sendNotificationHelper.SendNotificationAsync(new SendNotificationRequestDTO
                    {
                        Title = "New Service Request",
                        Body = $"You have received a new service request from {clientUser?.FullName}.",
                        DeviceToken = workerUser.DeviceToken
                    });
                }

                return job;


            }
            catch (Exception ex) {
               
                throw new Exception("Failed to create job request: " + ex.Message);

            }
        }

        
    }
}
