using ServanaAPP.Models;

namespace ServanaAPP.Interfaces
{
    public interface ITokenProvider
    {
        string CreateToken(User user);
    }
}
