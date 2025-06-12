using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.ClientHomeScreen.Request;
using ServanaAPP.Interfaces;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientHomeScreenController : ControllerBase
    {
        private readonly IHomeScreenClient _client;
        public ClientHomeScreenController(IHomeScreenClient homeScreenClient)
        {
            _client = homeScreenClient;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> getAllCategories()
        {
            try
            {
                var categories = await _client.AllCategories();
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
                var addCategories = await _client.AddCategory(input);
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
                var updateCategories = await _client.UpdateCategory(input);
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
                var deleteCategory = await _client.DeleteCategory(id);
                return Ok(deleteCategory);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}
