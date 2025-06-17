namespace ServanaAPP.DTOs.SendNotification.Request
{
    public class SendNotificationRequestDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string DeviceToken { get; set; }
    }
}
