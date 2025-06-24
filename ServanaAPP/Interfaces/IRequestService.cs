using ServanaAPP.DTOs.AcceptorReject;
using ServanaAPP.DTOs.RequestService.Request;
using ServanaAPP.Models;

namespace ServanaAPP.Interfaces
{
    public interface IRequestService
    {
       public Task <JobRequest> RequestServicee(RequestServiceDTO input);
       public Task<string> AcceptOrRejectRequestAsync(AcceptorRejectRequestDTO input);
    }
}
