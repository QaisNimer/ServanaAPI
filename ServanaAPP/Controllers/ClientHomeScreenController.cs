using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.ClientHomeScreen.Request;
using ServanaAPP.DTOs.ClientHomeScreen.Response;
using ServanaAPP.Interfaces;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientHomeScreenController : ControllerBase
    {
        private readonly IHomeScreenClient _homeScreenClient;
        public ClientHomeScreenController(IHomeScreenClient homeScreenClient)
        {
            _homeScreenClient = homeScreenClient;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> getAllCategories()
        {
            try
            {
                var categories = await _homeScreenClient.AllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCategory(AddCategoryRequestDTO input)
        {
            try
            {
                var addCategories = await _homeScreenClient.AddCategory(input);
                return Ok(addCategories);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCategory(ClientUpdateCAtegoryRequestDTO input)
        {
            try
            {
                var updateCategories = await _homeScreenClient.UpdateCategory(input);
                return Ok(updateCategories);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeletCategory(int id)
        {
            try
            {
                var deleteCategory = await _homeScreenClient.DeleteCategory(id);
                return Ok(deleteCategory);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchWorker(string name) {
            try
            {
                var searchWorker=await _homeScreenClient.SearchWorker(name);
                return Ok(searchWorker);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<TopRatedWorkerDTO>>> GetTopRatedWorkers()
        {
            try
            {
                var topWorkers = await _homeScreenClient.TopRatedWorkers();
                return Ok(topWorkers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong: {ex.Message}");
            }
        }
    }
}
