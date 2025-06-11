using ServanaAPP.DTOs.Signup.Request;
using ServanaAPP.DTOs.Verification.Request;
using ServanaAPP.Helpers.OtpUserSelection;
using ServanaAPP.Helpers.ValidationHelpers;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Services
{
    public class AuthServices : IAuthentication
    {
        private readonly ServanaDbContext _db;
        private readonly OtpBasedOnUserRole _otpBasedOnUserRole;
        public AuthServices(ServanaDbContext servanaDbContext, OtpBasedOnUserRole otpBasedOnUserRole) {
            _db= servanaDbContext;
            _otpBasedOnUserRole = otpBasedOnUserRole;
        }
        public Task<string> ResetPassword(ResetPasswordRequestDTO input)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendOTP(string email)
        {
            try
            {
                var user = _db.Users.Where(u => u.Email == email && u.IsLoggedIn == false).SingleOrDefault();
                if (user == null)
                {
                    return "Invalid Email";
                }
                user.OTP = null;
                user.ExpiryOTP = null;
                Random random = new Random();
                _db.Update(await _otpBasedOnUserRole.OTPBasedOnUserType(email, "OTP FOR RESET PASSWORD", "FOR RESET PASSWORD", user));
                _db.SaveChanges();
                return "Check your email OTP has been sent!";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Task<string> SignIn(SigninRequestDTO input)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SignUp(SignupRequestDTO input)
        {
            try
            {
                User user = new User();

                // Validate email and password
                if (!(ValidationHelpers.IsValidEmail(input.Email) || ValidationHelpers.IsValidatePassword(input.Password)))
                {
                    return "Not Valid Email or Password";
                }

                // Validate full name
                if (!ValidationHelpers.IsValidName(input.FullName))
                {
                    return "Not Valid FirstName or LastName";
                }

                // Handle profile image upload
                if (input.ProfileImageFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}_{input.ProfileImageFile.FileName}";
                    var uploadPath = Path.Combine("Uploads", fileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(uploadPath)!);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await input.ProfileImageFile.CopyToAsync(stream);
                    }

                    user.ProfileImage = fileName; // ✅ Store filename in DB
                }
                else
                {
                    user.ProfileImage = null; // Or set a default placeholder
                }

                // Set user info
                user.Email = input.Email;
                user.Password = input.Password;
                user.FullName = input.FullName;
                user.PhoneNumber = input.Phonenum;
                user.Role = input.Role;
                user.Gender = input.Gender;
                user.CreatedBy = "System";
                user.CreatedAt = DateTime.Now;

                _db.Users.Add(user);
                _db.SaveChanges();

                // Send OTP
                if (user.UserID > 0)
                {
                    _db.Update(await _otpBasedOnUserRole.OTPBasedOnUserType(
                        user.Email, "OTP for Sign Up.", "Completed Log Up.", user));

                    _db.SaveChanges();
                }

                return "Verifying your email using OTP";
            }
            catch (Exception ex)
            {

                return $"User Save Error: {ex.InnerException?.Message ?? ex.Message}";
            }
        }


        public Task<string> Verification(VerificationRequestDTO input)
        {
            throw new NotImplementedException();
        }
    }
}
