using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("WalksDifficulty")]
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
        [ActionName("GetWalkDifficultyAsync")]
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
        public async Task<IActionResult> AddWalksDifficultyAsync(WalkDifficulty addWalkDifficulty)
        {
            var walksdiff = await _walksDifficultyRepository.AddAsync(addWalkDifficulty);

            return Ok(walksdiff);


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
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] WalkDifficulty walk)
        {

            //Convert DTO to domain Model


            //Update region using repository
           var walkDifficulty = await _walksDifficultyRepository.UpdateAsync(id, walk);

            //if null then Notfound
            if (walk == null) { return NotFound(); }


            //Convert Domain back to DTO
            var walksDiffDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficulty.Code
            };

            //Return ok response
            return Ok(walksDiffDTO);
        }






    }
}
