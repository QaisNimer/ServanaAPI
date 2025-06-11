using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ServanaAPP.DTOs.Signup.Request;
using ServanaAPP.DTOs.Verification.Request;
using ServanaAPP.Helpers.JWT;
using ServanaAPP.Interfaces;
using System;
using System.Threading.Tasks;

namespace ServanaAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authService;
        private readonly GenerateJwtTokenHelper _jwtTokenHelper;
        public AuthenticationController(IAuthentication authentication, GenerateJwtTokenHelper generateJwtTokenHelper)
        {
            _authService = authentication;
            _jwtTokenHelper = generateJwtTokenHelper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignupRequestDTO input)
        {
            try
            {
                var token = await _authService.SignUp(input);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SigninRequestDTO input)
        {
            try
            {
                var token = await _authService.SignIn(input);
                if (string.IsNullOrEmpty(token) || token == "User not found")
                    return Unauthorized(new { message = "Invalid credentials." });
                return Ok(new { token = token });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP(string email)
        {
            try
            {
                var result = await _authService.SendOTP(email);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO input)
        {
            try
            {
                var result = await _authService.ResetPassword(input);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("verification")]
        public async Task<IActionResult> Verification(VerificationRequestDTO input)
        {
            try
            {
                var result = await _authService.Verification(input);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
