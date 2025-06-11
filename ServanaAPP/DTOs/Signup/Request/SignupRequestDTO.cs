using System.ComponentModel.DataAnnotations.Schema;

namespace ServanaAPP.DTOs.Signup.Request
{
    public class SignupRequestDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Phonenum { get; set; }

        public string FullName { get; set; }

        public char Gender { get; set; }

        public int Role { get; set; } = 2;

        [NotMapped]
        public IFormFile? ProfileImageFile { get; set; }
    }
}
