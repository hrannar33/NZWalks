using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("Regions")]
    public class RegionsController : Controller
    {
        private readonly IRegionsRepository _regionsRepository;

        private readonly IMapper Mapper;

        public RegionsController(IRegionsRepository regionsRepository, IMapper mapper)
        {
            this._regionsRepository = regionsRepository;
            this.Mapper = mapper;

        }

        [HttpGet]
        [Authorize(Roles = "reader")]

        public async Task<IActionResult> GetAllRegionsAsync()
        {

            var regions = await _regionsRepository.GetAllAsync();

            // Return DTO Regions

            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(regions =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = regions.Id, 
            //        Code = regions.Code,
            //        Name = regions.Name,
            //        Area = regions.Area,
            //        Lat = regions.Lat,  
            //        Long = regions.Long,
            //        Population = regions.Population,
            //    };
            //    regionsDTO.Add(regionDTO);


            //});


            var regionsDTO = Mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        [Authorize(Roles = "reader")]

        public async Task<IActionResult> GetRegionAsync(Guid id)
        {

            var result = await _regionsRepository.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            var response = Mapper.Map<Models.DTO.Region>(result);

            return Ok(response);

        }

        [HttpPost]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            // validate request
           //if(! ValidateAddRegionAsync(addRegionRequest)) return BadRequest(ModelState);

            //Request(DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };

            //Pass details to Repository
            region = await _regionsRepository.AddAsync(region);

            //Convert back to DTO
            var regionsDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionsDTO.Id }, regionsDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //get region from db
            var region = await _regionsRepository.DeleteAsync(id);


            //if null Notfound
            if (region == null)
            {
                return NotFound();
            }

            //Convert to DTO
            var regionsDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            //Return OK response

            return Ok(regionsDTO);



        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id ,[FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // Validate incoming request
           //if (!ValidateUpdateRegionAsync(updateRegionRequest)) return BadRequest(ModelState);

            //Convert DTO to domain Model
            var region = new Models.Domain.Region()
                {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population,
            };

            //Update region using repository
           region =  await _regionsRepository.UpdateAsync(id, region);

            //if null then Notfound
            if(region == null) { return NotFound(); }


            //Convert Domain back to DTO
            var regionsDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            //Return ok response
            return Ok(regionsDTO);
        }

        #region Private methods
        private Boolean ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null) {
                ModelState.AddModelError(nameof(addRegionRequest),
                   $"Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} cannot be null or have white spaces");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} cannot be null or have white spaces");
            }


            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero");
            }


            if (addRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} cannot be less than zero");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private Boolean ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                   $"Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} cannot be null or have white spaces");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} cannot be null or have white spaces");
            }


            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} cannot be less than or equal to zero");
            }


            if (updateRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} cannot be less than zero");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }


        #endregion

    }
}
