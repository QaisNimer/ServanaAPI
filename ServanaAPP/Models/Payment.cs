namespace ServanaAPP.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int RequestID { get; set; }
        public decimal TotalPrice { get; set; }
        public string Method { get; set; } // Cash or Wallet
        public DateTime PaidAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } // Client FullName
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = "System";

        public JobRequest JobRequest { get; set; }
    }

}
