using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
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
        public async  Task<IActionResult> GetAllRegions()
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


           var regionsDTO =  Mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

        }
    }
}
