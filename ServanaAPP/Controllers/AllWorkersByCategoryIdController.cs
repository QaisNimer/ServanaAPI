using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllWorkersByCategoryIdController : ControllerBase
    {
        private readonly IAllWorkersByCategoryId _allWorkersByCategoryId;
        public AllWorkersByCategoryIdController(IAllWorkersByCategoryId allWorkersByCategoryId)
        {
            _allWorkersByCategoryId = allWorkersByCategoryId;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<User>>> GetAllWorkersByCategoryId(int CategoryID)
        {
            try
            {
                var workers = await _allWorkersByCategoryId.GetAllWorkersByCategoryId(CategoryID);

                if (workers == null || !workers.Any())
                    return NotFound("No workers found for this category.");

                return Ok(workers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong: {ex.Message}");
            }
        }
    }
}
