using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.Api.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> logger;
        private IMailServices mail;
        ICityRepository cityRepository;
        public PointsOfInterestController(ILogger<PointsOfInterestController> _logger,IMailServices _mail,ICityRepository _cityRepository)
        {
            logger = _logger;
            mail = _mail;
            cityRepository = _cityRepository;
        }
        //return childern of the main object
        [HttpGet("{cityId}/pointsOfInterest")]
       public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!cityRepository.CityExist(cityId))
                {
                    logger.LogInformation($"city with {cityId} doesn't exist");
                    return NotFound();
                }
                var pointsofinterest = cityRepository.GetPointsOfInterests(cityId);
                var results = Mapper.Map<List<PointsOfInterestVM>>(pointsofinterest);
                return  Ok(results);
            }
            catch (Exception ex)
            {
                logger.LogCritical($"exception happend when getting data with {cityId}", ex);
                return StatusCode(500, "proplem happend when fetching data");
            }

        }
        //return a specific child of the specific object
        [HttpGet("{cityId}/pointsOfInterest/{id}")]
        public IActionResult GetPointOfInterest(int cityId,int id)
        {
            if (!cityRepository.CityExist(cityId))
                return NotFound();
            var pointsodinterest = cityRepository.GetPointsOfInterest(cityId, id);
            if (pointsodinterest == null)
                return NotFound();
            var result = Mapper.Map<PointsOfInterestVM>(pointsodinterest);
            return Ok(result);
        }
        //Create PointOfInterest
        [HttpPost("{cityId}/pointsOfInterest",Name ="pointOfInterest")]
        public IActionResult Create(int cityId,[FromBody]CreateUpdateCityOrPointsOfInterestVM createPointsOfInterest)
        {
            if (createPointsOfInterest == null)
                return BadRequest();
            if (createPointsOfInterest.Name == createPointsOfInterest.Description)
                ModelState.AddModelError("Description","Name should be different from Description");
            //check validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!cityRepository.CityExist(cityId))
                return NotFound();
            var point = Mapper.Map<Entity.PointsOfInterest>(createPointsOfInterest);
            cityRepository.CreatePointOfInterest(cityId,point);
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            var created = Mapper.Map<PointsOfInterestVM>(point);
            return CreatedAtRoute("pointOfInterest",new {cityId=cityId,id=created.Id},created);
        }
        //fully update pointOfInterest
        [HttpPut("{cityId}/pointsOfInterest/{id}")]
        public IActionResult FullyUpdate(int cityId,int id,[FromBody]CreateUpdateCityOrPointsOfInterestVM createUpdatePointsOfInterest)
        {
            if (createUpdatePointsOfInterest == null)
                return BadRequest();
            if (createUpdatePointsOfInterest.Name == createUpdatePointsOfInterest.Description)
                ModelState.AddModelError("Description", "Name should be different from Description");
            //check validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!cityRepository.CityExist(cityId))
                return NotFound();
            var pointOfInterest = cityRepository.GetPointsOfInterest(cityId,id);
            if (pointOfInterest == null)
                return NotFound();
            Mapper.Map(createUpdatePointsOfInterest, pointOfInterest);
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            return NoContent();
        }
        //partially update
        [HttpPatch("{cityId}/pointsOfInterest/{id}")]
        public IActionResult PartiallyUpdate(int cityId,int id,[FromBody] JsonPatchDocument<CreateUpdateCityOrPointsOfInterestVM> jsonPatch)
        {
            if (jsonPatch == null)
                return BadRequest();
            if (!cityRepository.CityExist(cityId))
                return NotFound();
            var pointOfInterest = cityRepository.GetPointsOfInterest(cityId,id);
            if (pointOfInterest == null)
                return NotFound();
            var pointOfInterestPatch = Mapper.Map<CreateUpdateCityOrPointsOfInterestVM>(pointOfInterest);
            jsonPatch.ApplyTo(pointOfInterestPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (pointOfInterestPatch.Name == pointOfInterestPatch.Description)
                ModelState.AddModelError("Description", "Name should be different from Description");
            TryValidateModel(pointOfInterestPatch);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Mapper.Map(pointOfInterestPatch,pointOfInterest);
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            return NoContent();
        }
        //Delete pointOfInterest
        [HttpDelete("{cityId}/pointsOfInterest/{id}")]
        public IActionResult Delete(int cityId, int id)
        {
            if (!cityRepository.CityExist(cityId))
                return NotFound();
            var pointOfInterest = cityRepository.GetPointsOfInterest(cityId,id);
            if (pointOfInterest == null)
                return NotFound();
            cityRepository.DeletePointOfInterest(pointOfInterest);
            mail.Send("delete",$"Point of name = {pointOfInterest.Name} deleted");
            if (!cityRepository.Save())
                return StatusCode(500, "proplem happend whern creating object");
            return NoContent();
        }
    }
}
