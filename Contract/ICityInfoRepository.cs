using CItyInfo.API.Entity;

namespace CItyInfo.API.Contract
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityByIdAsync(int cityId, bool includePointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestForCityAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        Task CreateCity(City city);
        Task CreatePointOfInterest(PointOfInterest pointOfInterest); 

    }
}
