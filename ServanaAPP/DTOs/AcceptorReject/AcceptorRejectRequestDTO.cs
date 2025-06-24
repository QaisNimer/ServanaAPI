namespace ServanaAPP.DTOs.AcceptorReject
{
    public class AcceptorRejectRequestDTO
    {
        public int RequestID { get; set; }
        public bool IsAccepted { get; set; } 
        public string UpdatedBy { get; set; }
    }
}
