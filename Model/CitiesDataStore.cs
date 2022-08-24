namespace CItyInfo.API.Model
{
    public class CitiesDataStore
    {
        public List<CityDto> NewCities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            //init dummy data

            NewCities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Lagos",
                    Description = "Centre of Excellence",
                    PointOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Victoria Island",
                            Description = "On the Island"
                        },
                        new PointOfInterestDto() 
                        { 
                            Id = 2,  
                            Name = "Ikeja",
                            Description = "Capital of the State"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Edo",
                    Description = "Heart Beat of the Nation",
                    PointOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Esan",
                            Description = "Home Town"
                        },
                        new PointOfInterestDto()
                        {
                            Id =4,
                            Name = "Benin",
                            Description = "Capital of the State"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Delta",
                    Description = "Big Heart",
                    PointOfInterests = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 5,
                            Name = "Warri",
                            Description = "Oil Well Region"
                        },
                        new PointOfInterestDto()
                        {
                            Id =6,
                            Name = "Asaba",
                            Description = "Capital of the State"
                        }
                    }
                }
            };
        }
    }
}
