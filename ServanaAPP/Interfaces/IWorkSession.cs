using ServanaAPP.DTOs.EndWork;
using ServanaAPP.DTOs.StartWork;

namespace ServanaAPP.Interfaces
{
    public interface IWorkSession
    {
        Task<string> StartWorkAsync(StartWorkDTO input);
        Task<string> EndWorkAsync(EndWorkDTO input);
    }
}
