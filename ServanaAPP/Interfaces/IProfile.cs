using ServanaAPP.DTOs.Profile.Request;

namespace ServanaAPP.Interfaces
{
    public interface IProfile
    {
        public Task<string> UpdateProfile(UpdateProfileRequestDTO input);
    }
}
