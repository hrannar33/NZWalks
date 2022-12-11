using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalksRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDBContext.Walk.AddAsync(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
                
         }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDBContext.Walk.FirstOrDefaultAsync(x => x.Id == id);
            if(walk == null)
            {
                return null;

            }

            nZWalksDBContext.Remove(walk);
            await nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
          return await  nZWalksDBContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();

        }

        public async Task<Walk> GetAsync(Guid id)
        {

            return await nZWalksDBContext.Walk
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {

            var existingWalk = await nZWalksDBContext.Walk.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null) return null;

            existingWalk.Length = walk.Length;
            existingWalk.Name = walk.Name;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            existingWalk.RegionId = walk.RegionId;
            await nZWalksDBContext.SaveChangesAsync();

            return existingWalk;

        }
    }
}
