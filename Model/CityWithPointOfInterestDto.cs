using CItyInfo.API.Entity;

namespace CItyInfo.API.Model
{
    public class CityWithPointOfInterestDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public City? City { get; set; }
        public int CityId { get; set; }
        public string? Description { get; set; }
    }
}
