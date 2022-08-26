using AutoMapper;
using CItyInfo.API.Contract;
using CItyInfo.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CItyInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cities")]

    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repo;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        public CitiesController(ICityInfoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities([FromQuery] string ? name, int pageSize = 10, int pageNumber = 1)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            if (!string.IsNullOrEmpty(name))
            {
                var filtereResult = await _repo.Filtering(name, pageNumber, pageSize);
                return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(filtereResult));
            }
            var cityEntites = await _repo.GetCitiesAsync(pageNumber, pageSize);
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntites));
        }
        /// <summary>
        /// Get a city by Id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointOfInterest">Whether to include the point of interest or not</param>
        /// <returns>An ActionResult</returns>
        
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType(int)HttpStatusCode.)]

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
            var mappedResult = _mapper.Map<CityWithoutPointOfInterestDto>(city);
            return Ok(mappedResult);
        }
    }

}
