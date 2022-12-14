using AutoMapper;
using CItyInfo.API.Entity;
using CItyInfo.API.Model;

namespace CItyInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestForCreatingDto, PointOfInterest>();
            CreateMap<PointOfInterestForUpdateDto, PointOfInterest>().ReverseMap();



        }
    }
}