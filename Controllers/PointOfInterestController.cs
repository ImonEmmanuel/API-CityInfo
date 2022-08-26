using AutoMapper;
using CItyInfo.API.Contract;
using CItyInfo.API.Entity;
using CItyInfo.API.Model;
using CItyInfo.API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CItyInfo.API.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointofinterest")]
    public class PointOfInterestController : Controller
    {
        private readonly ILogger _logger;
        private readonly ILocalMailService _mailServcie;
        private readonly CitiesDataStore _data;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _repo;
        public PointOfInterestController(ILogger<PointOfInterestController> logger,
            ILocalMailService mailService,IMapper mapper,ICityInfoRepository repo,
            CitiesDataStore data)
        {
            _logger = logger;
            _mailServcie = mailService;
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointOfInterest(int cityId)
        {
            try
            {
                //var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
                
                //var checkClaim = await _repo.CityNameMatchCityId(cityName, cityId);

                //if (!checkClaim)
                //{
                //    return Forbid();
                //}
                
                var city = _data.NewCities.Where(x => x.Id == cityId).FirstOrDefault();

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interes.");
                    return NotFound(); 
                }

                return Ok(city.PointOfInterests);
            }
            
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.",ex);
                return StatusCode(500, "A Problme happended while handling your request.");
                throw;
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {

            var cityById = await _repo.GetCityByIdAsync(cityId, false);
            if (cityById == null)
            {
                return NotFound();
            }
            var city = await _repo.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);


            if (city == null)
            {
                return NotFound();
            }
            

            return Ok(_mapper.Map<PointOfInterestDto>(city));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityid,[FromBody] PointOfInterestForCreatingDto pointOfInterest)
        {
   
            var city = _data.NewCities.FirstOrDefault(x => x.Id == cityid);
            if (city == null)
            {
                return NotFound();  
            }

            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);
            await _repo.AddPointOfInterestForCityAsync(cityid, finalPointOfInterest);
            await _repo.SaveChangesAsync();
            var createdPointOfInterst = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);


            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityid,
                    pointOfInterestId = createdPointOfInterst.Id
                }, createdPointOfInterst);
        }

        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult<PointOfInterestDto>> UpdatePointOfInterest(int cityId,int pointofinterestid, [FromBody] PointOfInterestForUpdateDto pointUpdate)
        {
            var x = string.Empty;
            var city = await _repo.GetCityByIdAsync(cityId, false);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterest = await _repo.GetPointOfInterestForCityAsync(cityId, pointofinterestid);

            if (pointOfInterest == null)
            {
                return NotFound();
            }
            _mapper.Map(pointUpdate, pointOfInterest);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
            int cityId, int pointofinterestid, 
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = await _repo.GetPointOfInterestForCityAsync(cityId, pointofinterestid);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestPatch = _mapper.Map<PointOfInterestForUpdateDto>(city);


            patchDocument.ApplyTo(pointOfInterestPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!TryValidateModel(pointOfInterestPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestPatch, city);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOFInterest(int cityId, int pointofinterestid)
        {
            var city = await _repo.GetCityByIdAsync(cityId, false);

            if (city == null)
            {
                return NotFound();
            }

            var pointToDelete = await _repo.GetPointOfInterestForCityAsync(cityId, pointofinterestid);


            if (pointToDelete == null)
            {
                return NotFound();
            }

            await _repo.DeletePointOfInterest(pointToDelete);

            _mailServcie.Send("Point of Interest deleted.", $"Point of Interest {pointToDelete.Name}" +
                $"with id {pointToDelete.Id} wass deleted from the database");
            return NoContent();

        }
    }
}
