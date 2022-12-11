using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{

    public class WalksDifficultyRepository : IWalksDifficultyRepository
    {
        private readonly NZWalksDBContext _nZWalksDBContext;

        public WalksDifficultyRepository(NZWalksDBContext nZWalksDBContext )
        {
            this._nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await _nZWalksDBContext.WalkDifficulty.AddAsync(walkDifficulty);
            await _nZWalksDBContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDiff = await _nZWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
            if(walkDiff == null)
            {
                return null;
            }
            _nZWalksDBContext.WalkDifficulty.Remove(walkDiff);
            await _nZWalksDBContext.SaveChangesAsync();
            return walkDiff;
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _nZWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAsyncAllWalkDifficulty()
        {
            return await _nZWalksDBContext
                .WalkDifficulty
                .ToListAsync ();
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDiff =  await _nZWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
             
            if(existingWalkDiff == null)
            {
                return null;
            }

            existingWalkDiff.Code = walkDifficulty.Code;
            await _nZWalksDBContext.SaveChangesAsync();

            return existingWalkDiff;

        }
    }
}
