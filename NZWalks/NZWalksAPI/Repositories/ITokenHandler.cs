using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface ITokenHandler
    {
       Task<String> CreateTokenAsync(User user);
    }
}
