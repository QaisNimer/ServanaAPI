using ServanaAPP.DTOs.ClientHomeScreen.Request;
using ServanaAPP.DTOs.ClientHomeScreen.Response;
using ServanaAPP.Models;

namespace ServanaAPP.Interfaces
{
    public interface IHomeScreenClient
    {
        public Task<List<User>> SearchWorker(string name);
        public Task<List<Category>> AllCategories();
        public Task<string> AddCategory(AddCategoryRequestDTO input);
        public Task<string> UpdateCategory(ClientUpdateCAtegoryRequestDTO input);
        public Task<string> DeleteCategory(int id);
        public Task<List<TopRatedWorkerDTO>> TopRatedWorkers();

    }
}
