using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class CityController : Controller
    {
        ICityRepository cityRepository;
        public CityController(ICityRepository _cityRepository)
        {
            cityRepository = _cityRepository;
        }
        [HttpGet()]
        //return all objects
        //IActionResult use instead of JsonResult to control the Http Status code 200,404,500
       public IActionResult GetCities()
        {
            var Cities = cityRepository.GetCities().ToList();
            List<CityWithoutPointsOfInterest> results = Mapper.Map<List<CityWithoutPointsOfInterest>>(Cities);
            return  Ok(results);
        }
        [HttpGet("{id}")]
        //return specific object
        public IActionResult GetCity(int id,bool includePoF=false)
        {
            var city = cityRepository.GetCity(id,includePoF);
            if (city == null)
                return NotFound();
            if (includePoF)
            {
                var cityResult = Mapper.Map<City>(city);
                return Ok(cityResult);
            }
            var cityRes = Mapper.Map<CityWithoutPointsOfInterest>(city);
            return  Ok(cityRes);
        }
    }
}
