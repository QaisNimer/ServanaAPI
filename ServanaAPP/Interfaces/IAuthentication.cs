using ServanaAPP.DTOs.Signup.Request;
using ServanaAPP.DTOs.Verification.Request;

namespace ServanaAPP.Interfaces
{
    public interface IAuthentication
    {
        Task<string> SignUp(SignupRequestDTO input);
        Task<string> SignIn(SigninRequestDTO input);
        Task<string> ResetPassword(ResetPasswordRequestDTO input);
        Task<string> SendOTP(string email);
        Task<string> Verification(VerificationRequestDTO input);
    }
}
