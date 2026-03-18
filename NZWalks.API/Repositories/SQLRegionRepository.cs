using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository: IRegionRepository
    {

        private readonly NZWalksDbContext DbContext;

       public SQLRegionRepository(NZWalksDbContext dbContext)
        {
                this.DbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await DbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await DbContext.Regions.AddAsync(region);
            await DbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionToUpdate = await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionToUpdate == null)
            {
                return null;
            }

            regionToUpdate.Code = region.Code;
            regionToUpdate.Name = region.Name;
            regionToUpdate.RegionImageUrl = region.RegionImageUrl;

            await DbContext.SaveChangesAsync();

            return regionToUpdate;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regiomDomain = await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regiomDomain == null)
                return null;

            DbContext.Regions.Remove(regiomDomain);
            await DbContext.SaveChangesAsync();

            return regiomDomain;
        }
    }
}
