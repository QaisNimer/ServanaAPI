namespace ServanaAPP.Models
{
    public class Rating
    {
        public int RatingID { get; set; }
        public int RequestID { get; set; }
        public int ClientID { get; set; }
        public int WorkerID { get; set; }
        public int Stars { get; set; }
        public string? Feedback { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } // Client FullName
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = "System";
        public JobRequest JobRequest { get; set; }
        public User Client { get; set; }
        public User Worker { get; set; }
    }

}
