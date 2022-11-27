using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly NZWalksDBContext _dbContext;

        public RegionsRepository(NZWalksDBContext nZWalksDBContext)
        {
            this._dbContext = nZWalksDBContext;
        }

        public async Task<Region> AddAsync(Region region)
        {

            region.Id = Guid.NewGuid();
    
            await _dbContext.Regions.AddAsync(region);
            await  _dbContext.SaveChangesAsync();
            return region;
                    
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(region == null)
            {
                return null;
            }
            //Delete region
            _dbContext.Regions.Remove(region);
            await _dbContext.SaveChangesAsync();

            return region;

        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
           return await _dbContext.Regions.ToListAsync();

        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await  _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
           var existingRegion =  await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existingRegion == null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

            await _dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
