namespace ServanaAPP.Models
{
    public class WorkSession
    {
        public int SessionID { get; set; }
        public int RequestID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal NumberOfWorkingHours { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal TotalCost => NumberOfWorkingHours * (HourlyRate ?? 0);
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = "System";

        public JobRequest JobRequest { get; set; }
    }

}
