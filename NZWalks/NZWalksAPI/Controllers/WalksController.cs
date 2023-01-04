using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using System.Xml;
using Newtonsoft.Json;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository walksRepository;

        private readonly IMapper Mapper;
        private readonly IRegionsRepository regionsRepository;
        private readonly IWalksDifficultyRepository walksDifficultyRepository;

        public WalksController(IWalksRepository walksRepository, IMapper mapper, IRegionsRepository regionsRepository, IWalksDifficultyRepository walksDifficultyRepository)
        {
            this.walksRepository = walksRepository;
            Mapper = mapper;
            this.regionsRepository = regionsRepository;
            this.walksDifficultyRepository = walksDifficultyRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //fetch data from database
            var walks = await walksRepository.GetAllAsync();
            //convert domain walks to DTO walks
            var walksDTO = Mapper.Map<List<Models.DTO.Walk>>(walks);

            string jsonWalksDTO = JsonConvert.SerializeObject(walksDTO, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            //return response

            return Ok(jsonWalksDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkByIdAsync")]
        public async Task<IActionResult> GetWalkByIdAsync(Guid id)
        {
            var result = await walksRepository.GetAsync(id);

            if (result == null)
            {
                return NotFound();

            }

            var response = Mapper.Map<Models.DTO.Walk>(result);

            string jsonWalksDTO = JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(jsonWalksDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalksAsync([FromBody] Models.DTO.AddWalkRequest addWalk)
        {
            if(!(await ValidateAddWalkAsync(addWalk))) return BadRequest(ModelState);

            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalk.Length,
                Name = addWalk.Name,
                RegionId = addWalk.RegionId,
                WalkDifficultyId = addWalk.WalkDifficultyId
            };

            walkDomain = await walksRepository.AddAsync(walkDomain);

            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };


            return CreatedAtAction(nameof(GetWalkByIdAsync), new { id = walkDTO.Id }, walkDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalksAsync(Guid id)
        {
            var walk = await walksRepository.DeleteAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            var response = Mapper.Map<Models.DTO.Walk>(walk);

            return Ok(response);


        }


        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalksAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //validate incoming request
            if (!(await ValidateupdatealkAsync(updateWalkRequest))) return BadRequest(ModelState);
            

            //Convert DTO to domain Model
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId

            };

            //Update region using repository
            walkDomain = await walksRepository.UpdateAsync(id, walkDomain);

            //if null then Notfound
            if (walkDomain == null) { return NotFound(); }

            else
            {
                //Convert Domain back to DTO
                var walksDTO = new Models.DTO.Walk
                {
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId,

                };
                return Ok(walksDTO);

            }

        }

        #region private methods


        private async Task<Boolean> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),
                   $"Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name),
                    $"{nameof(addWalkRequest.Name)} cannot be null or have white spaces");
            }

            if ((addWalkRequest.Length <= 0))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
                    $"{nameof(addWalkRequest.Length)} cannot be less than or equal to zero");
            }

            var region = await regionsRepository.GetAsync(addWalkRequest.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                     $"{nameof(addWalkRequest.RegionId)} is invalid");

            }
            var walkdifficulty = await walksDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);

            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                     $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");

            }


            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private async Task<Boolean> ValidateupdatealkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                   $"Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                    $"{nameof(updateWalkRequest.Name)} cannot be null or have white spaces");
            }

            if ((updateWalkRequest.Length <= 0))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                    $"{nameof(updateWalkRequest.Length)} cannot be less than or equal to zero");
            }

            var region = await regionsRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                     $"{nameof(updateWalkRequest.RegionId)} is invalid");

            }
            var walkdifficulty = await walksDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);

            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                     $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");

            }


            if (ModelState.ErrorCount > 0) return false;

            return true;
        }



        #endregion



    }
}
