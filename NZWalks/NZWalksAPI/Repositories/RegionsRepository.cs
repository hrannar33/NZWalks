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
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
           return await _dbContext.Regions.ToListAsync();

        }
    }
}
