using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IWalksDifficultyRepository
    {

        Task <IEnumerable<WalkDifficulty>> GetAsyncAllWalkDifficulty();

        Task<WalkDifficulty> GetAsync(Guid id);

        Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);

        Task<WalkDifficulty> DeleteAsync(Guid id);

        Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);


    }
}
