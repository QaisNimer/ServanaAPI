using System.ComponentModel.DataAnnotations;

namespace ServanaAPP.Models
{
    public class JobRequest
    {
        public int RequestID { get; set; }
        public int ClientID { get; set; }
        public int WorkerID { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; } // Cash or Wallet
        public string Status { get; set; } // Pending, Accepted, Started, Completed, Paid
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = "System";

        public User Client { get; set; }
        public User Worker { get; set; }
        public WorkSession WorkSession { get; set; }
        public Payment Payment { get; set; }
        public Rating Rating { get; set; }
    }

}
