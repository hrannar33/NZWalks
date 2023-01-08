using Microsoft.IdentityModel.Tokens;
using NZWalksAPI.Models.Domain;
using System.Threading.Tasks;

namespace NZWalksAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User>Authenticate(string username, string password);   
            
    }
}
