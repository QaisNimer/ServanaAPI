using Microsoft.EntityFrameworkCore;
using ServanaAPP.DTOs.EndWork;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.DTOs.StartWork;
using ServanaAPP.Helpers.Firebase;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Services
{
    public class WorkSessionService : IWorkSession
    {
        private readonly ServanaDbContext _db;
        private readonly SendNotificationHelper _sendNotificationHelper;

        public  WorkSessionService(ServanaDbContext db, SendNotificationHelper sendNotificationHelper)
        {
            _db = db;
            _sendNotificationHelper = sendNotificationHelper;
        }

        public async Task<string> EndWorkAsync(EndWorkDTO input)
        {
            var session = await _db.WorkSessions
                .Include(ws => ws.JobRequest)
                .ThenInclude(j => j.Worker) // get HourlyRate
                .FirstOrDefaultAsync(ws => ws.RequestID == input.RequestID && ws.IsActive);

            if (session == null)
                throw new Exception("Work session not found or inactive.");

            if (session.StartTime == null)
                throw new Exception("Work has not started yet.");

            var endTime = DateTime.Now;
            var totalHours = (decimal)(endTime - session.StartTime).TotalHours;
            session.NumberOfWorkingHours = Math.Round(totalHours, 2);
            session.HourlyRate = session.JobRequest.Worker?.PricePerHour ?? 0;
            session.UpdatedAt = endTime;
            session.UpdatedBy = input.UpdatedBy;

            session.JobRequest.Status = "completed";
            session.JobRequest.UpdatedAt = endTime;
            session.JobRequest.UpdatedBy = input.UpdatedBy;

            //Update job status to "pay"
            session.JobRequest.Status = "pay";
            session.JobRequest.UpdatedAt = endTime;
            session.JobRequest.UpdatedBy = input.UpdatedBy;

            await _db.SaveChangesAsync();
            var worker = await _db.Users
    .Where(u => u.UserID == session.JobRequest.WorkerID && u.IsActive)
    .Select(u => new { u.FullName })
    .FirstOrDefaultAsync();

            var client = await _db.Users
                .Where(u => u.UserID == session.JobRequest.ClientID && u.IsActive)
                .Select(u => new { u.FullName, u.DeviceToken })
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(client?.DeviceToken))
            {
                string workerName = worker?.FullName ?? "Worker";

                await _sendNotificationHelper.SendNotificationAsync(new SendNotificationRequestDTO
                {
                    Title = "Work Completed",
                    Body = $"{workerName} has ended the work session. Please proceed to payment.",
                    DeviceToken = client.DeviceToken
                });
            }
            return "Work ended successfully.";

        }

        public async Task<string> StartWorkAsync(StartWorkDTO input)
        {
            var session = await _db.WorkSessions
            .FirstOrDefaultAsync(ws => ws.RequestID == input.RequestID && ws.IsActive);

            if (session == null)
                throw new Exception("Work session not found or inactive.");

            

            session.StartTime = DateTime.Now;
            session.UpdatedAt = DateTime.Now;
            session.UpdatedBy = input.UpdatedBy;

            //Update job status to "working"
            session.JobRequest.Status = "working";
            session.JobRequest.UpdatedAt = DateTime.Now;
            session.JobRequest.UpdatedBy = input.UpdatedBy;

            await _db.SaveChangesAsync();
            // Notify worker that work has started
            var client = await _db.Users
    .Where(u => u.UserID == session.JobRequest.ClientID && u.IsActive)
    .Select(u => new { u.FullName })
    .FirstOrDefaultAsync();

            var worker = await _db.Users
                .Where(u => u.UserID == session.JobRequest.WorkerID && u.IsActive)
                .Select(u => new { u.FullName, u.DeviceToken })
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(worker?.DeviceToken))
            {
                string clientName = client?.FullName ?? "Client";

                await _sendNotificationHelper.SendNotificationAsync(new SendNotificationRequestDTO
                {
                    Title = "Work Started",
                    Body = $"{clientName} has started the work session.",
                    DeviceToken = worker.DeviceToken
                });
            }
            return "Work started successfully.";

            
            
        }
    }
}
