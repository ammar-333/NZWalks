using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // localhost:PORT/api/Walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        //  GET: /api/Walks?filterOn=Name&FilterQuery=Track&sortBy=Name&isAscending=true&PageNumber=1&pageSaze=5
        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? FilterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? IsAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var walks = await walkRepository.GetAllWalksAsync(filterOn, FilterQuery, sortBy, IsAscending ?? true, pageNumber, pageSize);

            var walkReturnDto = mapper.Map<List<WalkDto>>(walks);

            return Ok(walkReturnDto);
        }


        // GET: api/Walk/{id}
        [Route("{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walk = await walkRepository.GetWalkAsync(id);

            if (walk == null)
                return NotFound();

            var WalkReturnDto = mapper.Map<WalkDto>(walk);

            return Ok(WalkReturnDto);
        }

        // POST: /api/Walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkDto addWalkDto)
        {
            var walkDomain = mapper.Map<Walk>(addWalkDto);

            walkDomain = await walkRepository.CreateWalkAsync(walkDomain);

            var walkReturnDto = mapper.Map<WalkDto>(walkDomain);

            return Ok(walkReturnDto);
        }

        // PUT: api/Walks/{id}
        [Route("{id:guid}")]
        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            var walk = mapper.Map<Walk>(updateWalkDto);

            walk = await walkRepository.UpdateWalkAsync(id, walk);

            if (walk == null)
                return NotFound();

            var walkReturnDto = mapper.Map<WalkDto>(walk);

            return Ok(walkReturnDto);
        }

        // Delete: api/Walk/{id}
        [Route("{id:guid}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var walk = await walkRepository.DeleteWalkAsync(id);

            if (walk == null)
                return NotFound();

            var walkReturnDto = mapper.Map<WalkDto>(walk);

            return Ok(walkReturnDto);
        }

    }
}
