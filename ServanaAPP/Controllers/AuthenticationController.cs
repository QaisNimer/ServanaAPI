//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ServanaAPP.DTOs.Signup.Request;
//using ServanaAPP.DTOs.Verification.Request;
//using ServanaAPP.Helpers.OtpUserSelection;
//using ServanaAPP.Interfaces;
//using ServanaAPP.Models;
//using ServanaAPP.Services;

//namespace ServanaAPP.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthenticationController : ControllerBase
//    {
//        private readonly IAuthentication _authService;
//        public AuthenticationController(IAuthentication authentication) { 
//            _authService= authentication;
//        }
//        [HttpPost("{action}")]
//        public async Task<IActionResult> SignUp(SignupRequestDTO input) {
//            try
//            {
//                var token = await _authService.SignUp(input);
//                return StatusCode(200, "The OTP has been sent, Check You Email");
//            }
//            catch (Exception ex)
//            {

//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpPost("{action}")]
//        public async Task<IActionResult> OTP(string email)
//        {
//            try
//            {
//                return StatusCode(200, "The OTP has been sent, Check You Email");
//            }
//            catch (Exception ex)
//            {

//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpPost("{action}")]
//        public async Task<IActionResult> Verification(VerificationRequestDTO input)
//        {
//            try
//            {
//                return StatusCode(200, "Done");
//            }
//            catch (Exception ex)
//            {

//                return StatusCode(500, ex.Message);
//            }
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using ServanaAPP.DTOs.Signup.Request;
using ServanaAPP.DTOs.Verification.Request;
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

        public AuthenticationController(IAuthentication authentication)
        {
            _authService = authentication;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignupRequestDTO input)
        {
            try
            {
                var result = await _authService.SignUp(input);
                return Ok(new { message = result });
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
               /* var result = await _authService.SignIn(input);
                if (string.IsNullOrEmpty(result) || result == "User not found")
                    return Unauthorized(new { message = "Invalid credentials." });*/

                return Ok("loggedin");
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

