using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksDifficultyController : Controller
    {
        private readonly IWalksDifficultyRepository _walksDifficultyRepository;

        private readonly IMapper Mapper;

        public WalksDifficultyController(IWalksDifficultyRepository walksDifficultyRepository, IMapper mapper)
        {
            this._walksDifficultyRepository = walksDifficultyRepository;
            this.Mapper = mapper;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksDifficultiesAsync()
        {

            var walksDiff = await _walksDifficultyRepository.GetAsyncAllWalkDifficulty();


            var walkDifficultiesDTO = Mapper.Map<List<Models.DTO.WalkDifficulty>>(walksDiff);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyByIdAsync")]
        public async Task<IActionResult> GetWalkDifficultyByIdAsync(Guid id)
        {
            var result = await _walksDifficultyRepository.GetAsync(id);

            if(result == null) 
            {
                return NotFound();
            
            }

            var response = Mapper.Map<Models.DTO.WalkDifficulty>(result);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalksDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficulty)
        {
            var walkDiffDomain = new Models.Domain.WalkDifficulty { Code = addWalkDifficulty.Code };

            walkDiffDomain = await _walksDifficultyRepository.AddAsync(walkDiffDomain);

            var walkDifficultiesDTO = Mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);

            return CreatedAtAction(nameof(GetWalkDifficultyByIdAsync),
                new { id = walkDifficultiesDTO.Id },walkDifficultiesDTO);


        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalksDifficultyAsync(Guid id)
        {
            var walkDiff = await _walksDifficultyRepository.DeleteAsync(id);

            if(walkDiff == null) 
            { 
                return NotFound(); 
            }

            var response = Mapper.Map<Models.DTO.WalkDifficulty>(walkDiff);

            return Ok(response);


        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalk)
        {

            //Convert DTO to domain Model
            var walkDiffDomain = new Models.Domain.WalkDifficulty { Code = updateWalk.Code };

            //Update region using repository
            walkDiffDomain = await _walksDifficultyRepository.UpdateAsync(id, walkDiffDomain);

            //if null then Notfound
            if (walkDiffDomain == null) { return NotFound(); }


            //Convert Domain back to DTO
            var response = Mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);


            //Return ok response
            return Ok(response);
        }






    }
}
