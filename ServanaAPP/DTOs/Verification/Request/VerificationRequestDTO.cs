namespace ServanaAPP.DTOs.Verification.Request
{
    public class VerificationRequestDTO
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSignup { get; set; } = true;
    }
}
