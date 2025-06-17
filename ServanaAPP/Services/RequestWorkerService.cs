using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServanaAPP.DTOs.RequestWorker.Request;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Helpers.Firebase;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Services
{
    public class RequestWorkerService : IRequestWorker
    {
        private readonly ServanaDbContext _db;
        //private readonly SendNotificationHelper _sendNotificationHelper;
        public RequestWorkerService(ServanaDbContext db/*, SendNotificationHelper sendNotificationHelper*/)
        {
            _db = db;
            //_sendNotificationHelper = sendNotificationHelper;
        }
        public async Task<User> RequestServiceWorker(RequestWorkerDTOs input)
        {
            try
            {
                var requestWorker =await _db.Users.Where(w => w.UserID == input.WorkerId && w.IsActive && w.Role == 3).SingleOrDefaultAsync();
                if (requestWorker != null)
                {
                   
                    return requestWorker;
                }
                else
                {
                    throw new Exception("There's no worker available");
                }
            }
            catch (Exception ex)
            {

                throw new Exception (ex.Message);
            }
        }
    }
}
