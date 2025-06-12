using Microsoft.EntityFrameworkCore;
using ServanaAPP.DTOs.Signup.Request;
using ServanaAPP.DTOs.Verification.Request;
using ServanaAPP.Helpers.ImageHelpers;
using ServanaAPP.Helpers.JWT;
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
        private readonly GenerateJwtTokenHelper _jwtTokenHelper;
        public AuthServices(ServanaDbContext servanaDbContext, OtpBasedOnUserRole otpBasedOnUserRole, GenerateJwtTokenHelper jwtTokenHelper)
        {
            _db = servanaDbContext;
            _otpBasedOnUserRole = otpBasedOnUserRole;
            _jwtTokenHelper = jwtTokenHelper;
        }
        public async Task<string> ResetPassword(ResetPasswordRequestDTO input)
        {
            var user = _db.Users.Where(u => u.Email == input.Email && u.OTP == input.OTP
             && u.IsLoggedIn == false && u.ExpiryOTP > DateTime.Now).SingleOrDefault();
            if (user == null)
            {
                return "Go To SendOTPToResetPassword OR Signup if you DON'T have account";
            }
            if (input.NewPassword != input.ConfirmPassword)
            {
                return "Confirmation Of Password Failed !";
            }
            user.OTP = null;
            user.ExpiryOTP = null;
            user.Password = input.NewPassword;

            _db.Update(user);
            _db.SaveChanges();

            return "Your Password Updated Successfully Please Login Again.";
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
                _db.Update(await _otpBasedOnUserRole.OTPBasedOnUserType(email, "OTP FOR USER", "FOR USER NEW OTP", user));
                _db.SaveChanges();
                return "Check your email OTP has been sent!";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> SignIn(SigninRequestDTO input)
        {
            try
            {
                var user = await _db.Users.Where(u => u.Email == input.Email && u.Password == input.Password).FirstOrDefaultAsync();
                if (user == null)
                {
                    return $"No User Found";

                }
                if (!(ValidationHelpers.IsValidEmail(input.Email) || ValidationHelpers.IsValidEmail(input.Password)))
                {
                    return $"Not Valid Email or Password";
                }

                _db.Update(await _otpBasedOnUserRole.OTPBasedOnUserType(user.Email, "OTP for Sign In.", "Completed Log In.", user));
                _db.SaveChanges();
                return "Check your email OTP has been sent!";

            }
            catch (Exception ex)
            {
                return $" Can't SignIn Try Again{ex.Message}";
            }
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
                    string imagePath = await FileUploadHelper.UploadFileAsync(input.ProfileImageFile, "Uploads/UserProfileImages");
                    user.ProfileImage = imagePath;
                }
                else
                {
                    user.ProfileImage = null; 
                }

                user.Email = input.Email;
                user.Password = input.Password;
                user.FullName = input.FullName;
                user.PhoneNumber = input.Phonenum;
                user.Role = input.Role;
                user.Gender = input.Gender;
                user.CreatedBy = "System";
                user.CreatedAt = DateTime.Now;
                user.CategoryID = input.CategoryID;
                if ((user.Role==2 || user.Role==1) && user.CategoryID !=1)
                {
                    throw new Exception("You Are A Client Or Admin, Your Category Is 1");
                }
                if (user.Role==3 && user.CategoryID==1)
                {
                    throw new Exception("You Are A Worker, Your Category Is 2-6");
                }
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

        public async Task<string> Verification(VerificationRequestDTO input)
        {
            try
            {
                var user = _db.Users.Where(u => u.Email == input.Email && u.OTP == input.OTP
                           && u.IsLoggedIn == false && u.ExpiryOTP > DateTime.Now).SingleOrDefault();
                if (user == null)
                {
                    return "User not found";
                }

                if (input.IsSignup==true)
                {
                    user.IsVerified = true;
                    user.ExpiryOTP = null;
                    user.OTP = null;
                    user.IsLoggedIn = false;
                    input.IsSignup = false;
                    _db.Update(user);
                    _db.SaveChanges();
                    return "Your Account Is Verifyed";
                }
                else
                {
                    user.IsLoggedIn = true;
                    user.ExpiryOTP = null;
                    user.OTP = null;

                    _db.Update(user);
                    _db.SaveChanges();
                    var token = _jwtTokenHelper.CreateToken(user);
                    return token;

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
