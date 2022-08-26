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

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityByIdAsync(cityId, false);
            if (city != null)
            {
                city.PointOfInterests.Add(pointOfInterest);
            }
        }

        public async Task DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterests.Remove(pointOfInterest);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(int pageNumber, int pageSize)
        {
            return await _context.Cities.OrderBy(x => x.Name).Skip(pageSize * (pageNumber - 1)).ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int cityId, bool includePointOfInterest)
        {
            if (includePointOfInterest)
            {
                var result = await _context.Cities.Include( x => x.PointOfInterests).Where(x => x.Id == cityId).FirstOrDefaultAsync();
                return result;
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

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<City>> Filtering(string serachParameter, int pageNumber, int pageSize)
        {

            var result = await _context.Cities.Where(c => c.Name.Contains(serachParameter) || c.Description.Contains(serachParameter))
                .OrderBy(c => c.Name).Skip(pageSize * (pageNumber - 1)).ToListAsync();
            return result;
        }

        public async Task<bool> CityNameMatchCityId(string? cityName, int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }
    }
}
