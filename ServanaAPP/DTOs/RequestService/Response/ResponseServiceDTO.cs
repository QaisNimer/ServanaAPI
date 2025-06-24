namespace ServanaAPP.DTOs.RequestService.Response
{
    public class ResponseServiceDTO
    {
        public int RequestID { get; set; }
        public int ClientID { get; set; }
        public int WorkerID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
