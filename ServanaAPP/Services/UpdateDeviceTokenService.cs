using Microsoft.EntityFrameworkCore;
using ServanaAPP.DTOs.SendNotification.Request;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Services
{
    public class UpdateDeviceTokenService : IUpdateDeviceToken
    {
        private readonly ServanaDbContext _db;
        public UpdateDeviceTokenService(ServanaDbContext servanaDbContext) 
        {
        _db = servanaDbContext;
        }
        public async Task<string> UpdateDeviceToken(UpdateDeviceTokenRequestDTO input)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == input.UserId);
                if (user == null)
                    throw new Exception("User not found");

                user.DeviceToken = input.DeviceToken;
                user.UpdatedAt = DateTime.Now;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return "Device token updated successfully";

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
