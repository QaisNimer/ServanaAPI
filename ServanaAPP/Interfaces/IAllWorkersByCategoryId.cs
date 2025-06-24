using ServanaAPP.DTOs.GetAllWorkersByCategoryId.Request;
using ServanaAPP.Models;

namespace ServanaAPP.Interfaces
{
    public interface IAllWorkersByCategoryId
    {
        public Task<List<AllWorkersByCategoryIdDTO>> GetAllWorkersByCategoryId(int CategoryID);
    }
}
