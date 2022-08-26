using CItyInfo.API.Entity;

namespace CItyInfo.API.Contract
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync(int pageNumber, int pageSize);
        Task<City?> GetCityByIdAsync(int cityId, bool includePointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync(); 
        Task DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> CityNameMatchCityId(string? cityName, int cityId);

        Task<IEnumerable<City>> Filtering (string serachParameter, int pageNumber, int pageSize);

    }
}
