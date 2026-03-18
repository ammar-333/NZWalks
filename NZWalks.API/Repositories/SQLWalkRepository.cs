using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Net.WebSockets;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository: IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Walk> CreateWalkAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? FilterQuery = null,
            string? sortBy = null, bool IsAscending = true,
            int pageNumber = 1, int pageSize = 5)
        {
            //return await dbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();

            var walks = dbContext.Walks.Include(x => x.Region).Include(x => x.Difficulty).AsQueryable();

            //Filter
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(FilterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(FilterQuery));
                }
            }

            //Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.Name): walks.OrderByDescending(x => x.Name);
                }

                if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            // pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetWalkAsync(Guid id)
        {
            return await dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var walkToUpdate = await dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
            if (walkToUpdate == null)
                return null;

            walkToUpdate.Name = walk.Name;
            walkToUpdate.Description = walk.Name;
            walkToUpdate.LengthInKm = walk.LengthInKm;
            walkToUpdate.WalkImageUrl = walk.WalkImageUrl;
            walkToUpdate.DifficultyId = walk.DifficultyId;
            walkToUpdate.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();

            return walkToUpdate;


        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var walk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walk == null)
                return null;

            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

    }
}
