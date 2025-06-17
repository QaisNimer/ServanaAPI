using FirebaseAdmin.Messaging;
using ServanaAPP.DTOs.SendNotification.Request;

namespace ServanaAPP.Helpers.Firebase
{
    public class SendNotificationHelper
    {
        public async Task<string> SendNotificationAsync(SendNotificationRequestDTO input)
        {
            var message = new Message()
            {
                Token = input.DeviceToken,
                Notification = new Notification()
                {
                    Title = input.Title,
                    Body = input.Body,
                },
                Android = new AndroidConfig()
                {
                    Priority = Priority.High,
                },
                Apns = new ApnsConfig()
                {
                    Aps = new Aps()
                    {
                        ContentAvailable = true,
                    },
                },
            };
            //
            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    
    }
}
