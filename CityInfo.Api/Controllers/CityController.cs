using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
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
            List<CityWithoutPointsOfInterestVM> results = Mapper.Map<List<CityWithoutPointsOfInterestVM>>(Cities);
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
                var cityResult = Mapper.Map<CityVM>(city);
                return Ok(cityResult);
            }
            var cityRes = Mapper.Map<CityWithoutPointsOfInterestVM>(city);
            return  Ok(cityRes);
        }
        //Create City
        [HttpPost( Name = "city")]
        public IActionResult Create([FromBody]CreateUpdateCityOrPointsOfInterestVM createCity)
        {
            if (createCity == null)
                return BadRequest();
            if (createCity.Name == createCity.Description)
                ModelState.AddModelError("Description", "Name should be different from Description");
            //check validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var city = Mapper.Map<Entity.City>(createCity);
            cityRepository.CreateCity(city);
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            var created = Mapper.Map<CityVM>(city);
            return CreatedAtRoute("city", new {  id = created.Id }, created);
        }
        //fully update City
        [HttpPut("{cityId}")]
        public IActionResult FullyUpdate(int cityId,[FromBody]CreateUpdateCityOrPointsOfInterestVM createUpdateCity)
        {
            if (createUpdateCity == null)
                return BadRequest();
            if (createUpdateCity.Name == createUpdateCity.Description)
                ModelState.AddModelError("Description", "Name should be different from Description");
            //check validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var city = cityRepository.GetCity(cityId, false);
            if (city == null)
                return NotFound();
            Mapper.Map(createUpdateCity, city);
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            return NoContent();
        }
        //partially update
        [HttpPatch("{cityId}")]
        public IActionResult PartiallyUpdate(int cityId, [FromBody] JsonPatchDocument<CreateUpdateCityOrPointsOfInterestVM> jsonPatch)
        {
            if (jsonPatch == null)
                return BadRequest();
            var city = cityRepository.GetCity(cityId, false);
            if (city == null)
                return NotFound();
            var cityPatch = Mapper.Map<CreateUpdateCityOrPointsOfInterestVM>(city);
            jsonPatch.ApplyTo(cityPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (cityPatch.Name == cityPatch.Description)
                ModelState.AddModelError("Description", "Name should be different from Description");
            TryValidateModel(cityPatch);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Mapper.Map(cityPatch, city);
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            return NoContent();
        }
        //Delete City
        [HttpDelete("{cityId}")]
        public IActionResult Delete(int cityId)
        {
            var city = cityRepository.GetCity(cityId, false);
            if (city == null)
                return NotFound();
            cityRepository.Deletecity(city);
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            return NoContent();
        }
    }
}
