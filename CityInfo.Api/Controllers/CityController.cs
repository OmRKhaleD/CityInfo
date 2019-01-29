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
    public class CityController : Controller
    {
        [HttpGet()]
        //return all objects
        //IActionResult use instead of JsonResult to control the Http Status code 200,404,500
       public IActionResult GetCities()
        {
            return  Ok(CityMock.Current.Cities);
        }
        [HttpGet("{id}")]
        //return specific object
        public IActionResult GetCity(int id)
        {
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
                return NotFound();
            return  Ok(city);
        }
    }
}
