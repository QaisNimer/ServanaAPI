using ServanaAPP.DTOs.RequestWorker.Request;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Models;

namespace ServanaAPP.Interfaces
{
    public interface IRequestWorker
    {
        public Task<User> RequestServiceWorker(RequestWorkerDTOs input);
    }
}
