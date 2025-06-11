using ServanaAPP.Helpers.SendingEmail;
using ServanaAPP.Models;

namespace ServanaAPP.Helpers.OtpUserSelection
{
    public class OtpBasedOnUserRole
    {
        private readonly MailingHelper _mailingHelper;
        public OtpBasedOnUserRole(MailingHelper mailingHelper)
        {
            _mailingHelper = mailingHelper;
        }
        public async Task<User> OTPBasedOnUserType(string Email, string title, string message, User us)
        {
            var otp = 111111;
            var expireDate = DateTime.Now;
            Random rand = new Random();
            otp = rand.Next(11111, 99999);
            expireDate = DateTime.Now.AddMinutes(10);
            us.OTP = otp.ToString();
            us.ExpiryOTP = expireDate;
            await _mailingHelper.SendEmail(us.Email, us.OTP, title, message);
            return us;

        }
    }
}
