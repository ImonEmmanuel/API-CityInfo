using CItyInfo.API.Model;
using CItyInfo.API.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CItyInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointofinterest")]
    public class PointOfInterestController : Controller
    {
        private readonly ILogger _logger;
        private readonly ILocalMailService _mailServcie;
        private readonly CitiesDataStore _data;
        public PointOfInterestController(ILogger<PointOfInterestController> logger,
            ILocalMailService mailService,
            CitiesDataStore data)
        {
            _logger = logger;
            _mailServcie = mailService;
            _data = data;
        }

        [HttpGet]

        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId)
        {
            try
            {
                
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
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {

            var cityById = _data.NewCities.FirstOrDefault(x => x.Id == cityId);
            if (cityById == null)
            {
                return NotFound();
            }
            var city = _data.NewCities.FirstOrDefault(x => x.Id == cityId)
                .PointOfInterests.FirstOrDefault(y => y.Id == pointOfInterestId);


            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityid,[FromBody] PointOfInterestForCreatingDto pointOfInterest)
        {
   
            var city = _data.NewCities.FirstOrDefault(x => x.Id == cityid);
            if (city == null)
            {
                return NotFound();  
            }

            var maxPoint = _data.NewCities.SelectMany(c => c.PointOfInterests).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPoint,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            
            city.PointOfInterests.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityid,
                    pointOfInterestId = finalPointOfInterest.Id
                }, finalPointOfInterest);
        }

        [HttpPut("{pointofinterestid}")]
        public ActionResult<PointOfInterestDto> UpdatePointOfInterest(int cityId,int pointofinterestid, [FromBody] PointOfInterestForUpdateDto pointUpdate)
        {
            var city = _data.NewCities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointCity = city.PointOfInterests.FirstOrDefault(x => x.Id == pointofinterestid);
            if (pointCity == null)
            {
                return NotFound();
            }

            pointCity.Name = pointUpdate.Name;
            pointCity.Description = pointUpdate.Description;

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId, int pointofinterestid, 
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _data.NewCities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointInterest = city.PointOfInterests.FirstOrDefault(x => x.Id == pointofinterestid);
            if (pointInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointInterest.Name,
                Description = pointInterest.Description
            };
            patchDocument.ApplyTo(pointOfInterestPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!TryValidateModel(pointOfInterestPatch))
            {
                return BadRequest(ModelState);
            }

            pointInterest.Name = pointOfInterestPatch.Name;
            pointInterest.Description = pointOfInterestPatch.Description;


            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public ActionResult DeletePointOFInterest(int cityId, int pointofinterestid)
        {
            var city = _data.NewCities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointToDelete = city.PointOfInterests.FirstOrDefault(x => x.Id == pointofinterestid);

            if (pointToDelete == null)
            {
                return NotFound();
            }

            city.PointOfInterests.Remove(pointToDelete);
            _mailServcie.Send("Point of Interest deleted.", $"Point of Interest {pointToDelete.Name}" +
                $"with id {pointToDelete.Id} wass deleted from the database");
            return NoContent();

        }
    }
}
