using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    // //localhost:port/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionReopsitory;
        private readonly ILogger<RegionsController> logger;
        private readonly IMapper mapper;

        public RegionsController(IMapper mapper, IRegionRepository regionReopsitory, ILogger<RegionsController> logger)
        {
            this.regionReopsitory = regionReopsitory;
            this.logger = logger;
            this.mapper = mapper;
        }

        // GET: localhost:port/api/Regions
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {

            logger.LogInformation("GetAll Regions Action Method Was Invoked");

            var regionsDomains = await regionReopsitory.GetAllAsync();

            logger.LogInformation($"Finished GetAll Regions Request with Data {JsonSerializer.Serialize(regionsDomains)}");

            var regionDto = mapper.Map<List<RegionDto>>(regionsDomains);

            return Ok(regionDto);
        }

        // GET: localhost:port/api/Regions/{id}
        [Route("{id:guid}")]
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var region = await regionReopsitory.GetByIdAsync(id);

            if (region == null)
                return NotFound();

            var regionDto = mapper.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        //POST: localhost:port/api/Regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionDto addRegionDto)
        {

            var regionDomain = mapper.Map<Region>(addRegionDto);

            regionDomain = await regionReopsitory.CreateAsync(regionDomain);

            var regionDtoToReturn = mapper.Map<RegionDto>(regionDomain);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomain.Id }, regionDtoToReturn);
        }

        // PUT: localhost:port/api/Regions/{id}
        [Route("{id:guid}")]
        [HttpPut]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {

            var region = mapper.Map<Region>(updateRegionDto);

            var regionDomain = await regionReopsitory.UpdateAsync(id, region);

            if (regionDomain == null)
                return NotFound();

            var regionDtoToReturn = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDtoToReturn);

        }

        // DELETE: localhost:port/api/Regions/{id}
        [Route("{id:guid}")]
        [HttpDelete]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regiomDomain = await regionReopsitory.DeleteAsync(id);
            if (regiomDomain == null)
                return NotFound();

            var regionDtoToReturn = mapper.Map<RegionDto>(regiomDomain);

            return Ok(regionDtoToReturn);
        }

    }
}
