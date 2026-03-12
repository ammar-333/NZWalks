using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {

        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? FilterQuery = null, string? sortBy = null, bool IsAscending = true, int pageNumber = 1, int pageSize = 5);
        Task<Walk?> GetWalkAsync(Guid id);
        Task<Walk> CreateWalkAsync(Walk walk);
        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
