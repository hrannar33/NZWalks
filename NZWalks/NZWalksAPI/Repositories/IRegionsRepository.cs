using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IRegionsRepository
    {
      Task <IEnumerable<Region>> GetAllAsync();
    }
}
