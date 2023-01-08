using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Data;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "reader")]

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
        [Authorize(Roles = "reader")]

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
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> AddWalksDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficulty)
        {
            //validate request
            //if (!ValidateAddWalkDifficultykAsync(addWalkDifficulty)) return BadRequest(ModelState);

            var walkDiffDomain = new Models.Domain.WalkDifficulty { Code = addWalkDifficulty.Code };

            walkDiffDomain = await _walksDifficultyRepository.AddAsync(walkDiffDomain);

            var walkDifficultiesDTO = Mapper.Map<Models.DTO.WalkDifficulty>(walkDiffDomain);

            return CreatedAtAction(nameof(GetWalkDifficultyByIdAsync),
                new { id = walkDifficultiesDTO.Id },walkDifficultiesDTO);


        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

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
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalk)
        {

            //validate request
            //if (!ValidateupdateWalkDifficultyAsync(updateWalk)) return BadRequest(ModelState);


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


        #region private methods


        private  Boolean ValidateAddWalkDifficultykAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                   $"Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                    $"{nameof(addWalkDifficultyRequest.Code)} cannot be null or have white spaces");
            }


            if (ModelState.ErrorCount > 0) return false;

            return true;
        }

        private Boolean ValidateupdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                   $"Add Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null or have white spaces");
            }

            if (ModelState.ErrorCount > 0) return false;

            return true;
        }



        #endregion



    }
}
