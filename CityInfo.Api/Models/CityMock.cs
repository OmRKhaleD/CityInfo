using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Models
{
    public class CityMock
    {
        public static CityMock Current { get; } = new CityMock();
        public List<CityVM> Cities { get; set; }

        public CityMock()
        {
            // init dummy data
            Cities = new List<CityVM>()
            {
                new CityVM()
                {
                     Id = 1,
                     Name = "New York City",
                     Description = "The one with that big park.",
                     PointsOfInterest = new List<PointsOfInterestVM>()
                     {
                         new PointsOfInterestVM() {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States." },
                          new PointsOfInterestVM() {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }
                },
                new CityVM()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished.",
                    PointsOfInterest = new List<PointsOfInterestVM>()
                     {
                         new PointsOfInterestVM() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointsOfInterestVM() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                },
                new CityVM()
                {
                    Id= 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    PointsOfInterest = new List<PointsOfInterestVM>()
                     {
                         new PointsOfInterestVM() {
                             Id = 5,
                             Name = "Eiffel Tower",
                             Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PointsOfInterestVM() {
                             Id = 6,
                             Name = "The Louvre",
                             Description = "The world's largest museum." },
                     }
                }
            };

        }
    }
}
