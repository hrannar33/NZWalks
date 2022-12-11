using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IWalksRepository
    {
        Task<IEnumerable<Walk>> GetAllAsync();

        Task<Walk> GetAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
        Task<Walk> AddAsync(Walk walk);
        Task<Walk> DeleteAsync(Guid id);
    }
}
