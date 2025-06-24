using Microsoft.EntityFrameworkCore;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Helpers.Firebase;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ServanaDbContext _db;
        private readonly SendNotificationHelper _sendNotificationHelper;
        public PaymentService(ServanaDbContext db,SendNotificationHelper sendNotificationHelper)
        {
            _db = db;
            _sendNotificationHelper = sendNotificationHelper;
        }
        public async Task<string> HandlePaymentAsync(int requestId, string method)
        {
            var job = await _db.JobRequests
                 .Include(j => j.WorkSession)
                 .FirstOrDefaultAsync(j => j.RequestID == requestId && j.IsActive);

            if (job == null || job.WorkSession == null)
                throw new Exception("Invalid request or work session not found");

            var worker = await _db.Users.FindAsync(job.WorkerID);
            var client = await _db.Users.FindAsync(job.ClientID);
            if (worker == null || client == null)
                throw new Exception("Invalid user data");

            // Calculate totals
            decimal total = job.WorkSession.TotalCost;
            decimal commission = total * 0.1m;
            decimal earnings = total - commission;

            if (method.ToLower() == "wallet")
            {
                if (client.WalletBalance < total)
                    throw new Exception("Insufficient wallet balance");

                client.WalletBalance -= total;
            }

            // Save payment
            var payment = new Payment
            {
                RequestID = job.RequestID,
                TotalPrice = total,
                Method = method,
                CreatedBy = client.FullName
            };

            _db.Payments.Add(payment);

            // Update job & session
            job.Status = "Completed";
            job.IsActive = false;
            job.UpdatedAt = DateTime.Now;
            job.UpdatedBy = "System";

            job.WorkSession.IsActive = false;
            job.WorkSession.UpdatedAt = DateTime.Now;
            job.WorkSession.UpdatedBy = "System";

            await _db.SaveChangesAsync();

            //Send Notification to Worker
            var worker1 = await _db.Users
                .Where(w => w.UserID == job.WorkerID && w.IsActive)
                .Select(w => new { w.FullName, w.DeviceToken })
                .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(worker1?.DeviceToken))
            {
                

                await _sendNotificationHelper.SendNotificationAsync(new SendNotificationRequestDTO
                {
                    Title = "Payment Received",
                    Body = $"Payment has been completed.",
                    DeviceToken = worker1.DeviceToken
                });
            }

            //Send Notification to Client
            if (!string.IsNullOrEmpty(client?.DeviceToken))
            {
                await _sendNotificationHelper.SendNotificationAsync(new SendNotificationRequestDTO
                {
                    Title = "Service Completed",
                    Body = "Thank you! Your service has been completed successfully.",
                    DeviceToken = client.DeviceToken
                });
            }
            return "Payment completed successfully.";



        }
    }
}
