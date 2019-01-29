using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        //return childern of the main object
        [HttpGet("{cityId}/pointsOfInterest")]
       public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            return  Ok(city.PointsOfInterest);
        }
        //return a specific child of the specific object
        [HttpGet("{cityId}/pointsOfInterest/{id}")]
        public IActionResult GetPointOfInterest(int cityId,int id)
        {
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
                return NotFound();
            return  Ok(pointOfInterest);
        }
    }
}
