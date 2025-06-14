using Microsoft.EntityFrameworkCore;
using ServanaAPP.DTOs.Profile.Request;
using ServanaAPP.Helpers.ImageHelpers;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;
using System.Security.Claims;

namespace ServanaAPP.Services
{
    public class ProfileService : IProfile
    {

        private readonly ServanaDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(ServanaDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UpdateProfile(UpdateProfileRequestDTO input)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Unauthorized: User ID not found in token");

                int parsedUserId = int.Parse(userId);
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == parsedUserId);

                if (user == null)
                {
                    throw new Exception("User not found");
                }
                user.FullName = input.FullName;
                user.PhoneNumber = input.PhoneNumber;

                if (input.ProfileImage != null)
                {

                    string imagePath = await FileUploadHelper.UploadFileAsync(input.ProfileImage, "Uploads/ProfilePageImages");
                    user.ProfileImage = imagePath;

                }
                user.AddressTitle = input.AddressTitle;
                user.Latitude = input.Latitude;
                user.Longitude = input.Longitude;

                if (user.Role == 3)
                {
                    user.PricePerHour = input.PricePerHour;
                    user.CategoryID = input.CategoryID;
                }

                await _db.SaveChangesAsync();

                return "Profile updated successfully.";

            }
            catch (Exception ex) { 
              throw new Exception(ex.Message);
            
            }
        }
    }
}
