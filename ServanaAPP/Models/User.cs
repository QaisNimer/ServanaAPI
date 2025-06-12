using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace ServanaAPP.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public char Gender { get; set; }
        [NotMapped] 
        public IFormFile? ProfileImageFile { get; set; } // Used only for upload
        public string? ProfileImage { get; set; }
        public int Role { get; set; } // 1 = Admin, 2 = Client, 3 = Worker
        public decimal ?PricePerHour { get; set; }
        public decimal WalletBalance { get; set; } = 0;
        public bool IsVerified { get; set; } = false;
        public string? AddressTitle { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "System";
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = "System";
        public string ?OTP {  get; set; }
        public DateTime ?ExpiryOTP {  get; set; }
        public bool IsLoggedIn { get; set;} = false;
        public int CategoryID { get; set; } // For Category Table

        public ICollection<JobRequest> JobRequestsSent { get; set; }
        public ICollection<JobRequest> JobRequestsReceived { get; set; }
        public ICollection<Rating> RatingsGiven { get; set; }
        public ICollection<Rating> RatingsReceived { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
    }
}
