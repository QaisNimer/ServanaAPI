namespace ServanaAPP.DTOs.RequestService.Request
{
    public class RequestServiceDTO
    {
        public int clientID { get; set; }
        public int workerID { get; set; }
        public string description { get; set; }
        public string status { get; set; } = "pending";

    }
}
