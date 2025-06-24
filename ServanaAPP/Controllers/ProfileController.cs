using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.Profile.Request;
using ServanaAPP.Interfaces;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfile _profileService;

        public ProfileController(IProfile profileService)
        {
            _profileService = profileService;
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequestDTO input)
        {
            try
            {
                var result = await _profileService.UpdateProfile(input);
                return Ok(new { message = result });
            }
            catch(Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }
    }
}
