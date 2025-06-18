using ServanaAPP.DTOs.SendNotification.Request;

namespace ServanaAPP.Interfaces
{
    public interface IUpdateDeviceToken
    {
        public Task<string> UpdateDeviceToken(UpdateDeviceTokenRequestDTO input);

    }
}
