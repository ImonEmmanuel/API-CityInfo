using AutoMapper;
using CItyInfo.API.Entity;
using CItyInfo.API.Model;

namespace CItyInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointOfInterestDto>();
        }
    }
}
