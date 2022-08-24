using CItyInfo.API.Entity;
using CItyInfo.API.Model;
using CItyInfo.API.Model.InitialData;
using Microsoft.EntityFrameworkCore;

namespace CItyInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {

        }
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<City>().HasData(InitialCity.GetCityData());
            modelBuilder.Entity<PointOfInterest>().HasData(InitialCity.GetPointOfInterestData());

            base.OnModelCreating(modelBuilder);
        }

    }
}
