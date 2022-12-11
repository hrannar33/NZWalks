using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using System.Xml;
using Newtonsoft.Json;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository walksRepository;

        private readonly IMapper Mapper;

        public WalksController(IWalksRepository walksRepository, IMapper mapper)
        {
            this.walksRepository = walksRepository;
            Mapper = mapper;
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





    }
}
