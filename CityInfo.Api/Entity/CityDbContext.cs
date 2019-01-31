using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Entity
{
    public class CityDbContext: DbContext
    {
        public CityDbContext(DbContextOptions<CityDbContext> options):base(options)
        {
            Database.Migrate();
        }
        public DbSet<City> City { get; set; }
        public DbSet<PointsOfInterest> PointsOfInterest { get; set; }
    }
}
