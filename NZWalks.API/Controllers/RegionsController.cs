using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;

namespace NZWalks.API.Controllers
{
    // //localhost:port/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: localhost:port/api/Regions
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomains = dbContext.Regions.ToList();

            var redionsDto = new List<RegionDto>();
            regionsDomains.ForEach(regionDomain =>
            {
                var regionDto = new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                };
                redionsDto.Add(regionDto);
            });

            return Ok(redionsDto);
        }

        // GET: localhost:port/api/Regions/{id}
        [Route("{id:guid}")]
        [HttpGet]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
                return NotFound();

            var regionDto = new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDto);
        }

        //POST: localhost:port/api/Regions
        [HttpPost]
        public IActionResult CreateRegion([FromBody] AddRegionDto AddRegionDto)
        {
            var regionDomain = new Region()
            {
                Code = AddRegionDto.Code,
                Name = AddRegionDto.Name,
                RegionImageUrl = AddRegionDto.RegionImageUrl
            };

            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();

            var regionDtoToReturn = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomain.Id }, regionDtoToReturn);
        }

        // PUT: localhost:port/api/Regions/{id}
        [Route("{id:guid}")]
        [HttpPut]
        public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            var regiomDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regiomDomain == null)
                return NotFound();

            regiomDomain.Code = updateRegionDto.Code;
            regiomDomain.Name = updateRegionDto.Name;
            regiomDomain.RegionImageUrl = updateRegionDto.RegionImageUrl;

            dbContext.SaveChanges();

            var regionDtoToReturn = new RegionDto()
            {
                Id = regiomDomain.Id,
                Code = regiomDomain.Code,
                Name = regiomDomain.Name,
                RegionImageUrl = regiomDomain.RegionImageUrl
            };

            return Ok(regionDtoToReturn);

        }

        // DELETE: localhost:port/api/Regions/{id}
        [Route("{id:guid}")]
        [HttpDelete]
        public IActionResult DeleteRegion([FromRoute] Guid id)
        {
            var regiomDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regiomDomain == null)
                return NotFound();

            dbContext.Regions.Remove(regiomDomain);
            dbContext.SaveChanges();

            var regionDtoToReturn = new RegionDto()
            {
                Id = regiomDomain.Id,
                Code = regiomDomain.Code,
                Name = regiomDomain.Name,
                RegionImageUrl = regiomDomain.RegionImageUrl
            };

            return Ok(regionDtoToReturn);
        }

    }
}
