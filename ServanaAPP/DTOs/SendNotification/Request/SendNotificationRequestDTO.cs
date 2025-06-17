namespace ServanaAPP.DTOs.SendNotification.Request
{
    public class SendNotificationRequestDTO
    {
        public int? WorkerId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string DeviceToken { get; set; }
    }
}
