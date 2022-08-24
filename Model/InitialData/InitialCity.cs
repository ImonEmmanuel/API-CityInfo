using CItyInfo.API.Entity;

namespace CItyInfo.API.Model.InitialData
{
    public static class InitialCity
    {
        public static IEnumerable<City> GetCityData()
        {
            List<City> cityList = new()
             {
                 new City()
                 {
                     Id = 1,
                     Name = "Lagos",
                     Description = "Centre of Excellence"
                 },
                 new City()
                 {
                     Id = 2,
                     Name = "Edo",
                     Description = "Heart Beat of the Nation"
                 },
                 new City()
                 {
                     Id = 3,
                     Name = "Delta",
                     Description = "Big Heart"
                 }

             };
            return cityList;
        }

        public static IEnumerable<PointOfInterest> GetPointOfInterestData()
        {
            List<PointOfInterest> pointOfInterest = new()
            {
                new PointOfInterest()
                {
                    Id = 1,
                    CityId =1,
                    Description = "High Rise Buildings and Water Log",
                    Name = "Island"
                },
                new PointOfInterest()
                {
                    Id = 2,
                    CityId =1,
                    Description = "Capital City of the State",
                    Name = "Mainland"
                },
                new PointOfInterest()
                {
                    Id = 3,
                    CityId =2,
                    Description = "Capital City of the State",
                    Name = "Benin"
                },
                new PointOfInterest()
                {
                    Id = 4,
                    CityId =2,
                    Description = "Home Town",
                    Name = "Esan"
                },
                new PointOfInterest()
                {
                    Id = 5,
                    CityId =3,
                    Description = "Capital City of the State",
                    Name = "Asaba"
                },
                new PointOfInterest()
                {
                    Id = 6,
                    CityId =3,
                    Description = "Oil Rich Region",
                    Name = "Warri"
                }
            };
            return pointOfInterest;
        }
    }
}
