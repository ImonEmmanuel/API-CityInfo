using AutoMapper;
using CItyInfo.API.Contract;
using CItyInfo.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace CItyInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]

    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repo;
        private readonly IMapper _mapper;
        public CitiesController(ICityInfoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet ]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
        {
            var cityEntites = await _repo.GetCitiesAsync();
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntites));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCity(int id, bool includePointOfInterest = false)
        {
            var city = await _repo.GetCityByIdAsync(id, includePointOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointOfInterest)
            { 
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
        }
    }

}
