using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace ServanaAPP.Helpers.SendingEmail
{
    public class MailingHelper
    {
        private readonly SendGridSettings _settings;
        public MailingHelper(IOptions<SendGridSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmail(string email, string otp, string title, string message)
        {
            try
            {
                var apiKey = _settings.ApiKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("apptraines@gmail.com", "Servana");
                var subject = title;
                var to = new EmailAddress(email, "Password Manager User");
                var plainTextContent = $"Dear User {message} Please Use Tis OTP {otp} It Will Be Expired After 10 Minutes ";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, "");
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
