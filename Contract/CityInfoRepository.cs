using CItyInfo.API.DbContexts;
using CItyInfo.API.Entity;
using Microsoft.EntityFrameworkCore;

namespace CItyInfo.API.Contract
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public Task CreateCity(City city)
        {
            throw new NotImplementedException();
        }

        public Task CreatePointOfInterest(PointOfInterest pointOfInterest)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                return await _context.Cities.Include( x => x.PointOfInterests).Where(x => x.Id == cityId).FirstOrDefaultAsync(); 
            }
            return await _context.Cities.FirstOrDefaultAsync(x => x.Id == cityId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterests.Where(x => x.CityId == cityId).ToListAsync();
        }

        public async Task<PointOfInterest> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointOfInterests.Where(x => x.CityId == cityId && x.Id == pointOfInterestId).FirstOrDefaultAsync();
        }
    }
}
