using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public PointsOfInterestController(ILogger<PointsOfInterestController> _logger,IMailServices _mail)
        {
            logger = _logger;
            mail = _mail;
        }
        //return childern of the main object
        [HttpGet("{cityId}/pointsOfInterest")]
       public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                throw new Exception("Exception");
                var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    logger.LogInformation($"city with {cityId} doesn't exist");
                    return NotFound();
                }
                return  Ok(city.PointsOfInterest);
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
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
                return NotFound();
            return  Ok(pointOfInterest);
        }
        //Create PointOfInterest
        [HttpPost("{cityId}/pointsOfInterest",Name ="pointOfInterest")]
        public IActionResult Create(int cityId,[FromBody]CreateUpdatePointsOfInterest createPointsOfInterest)
        {
            if (createPointsOfInterest == null)
                return BadRequest();
            if (createPointsOfInterest.Name == createPointsOfInterest.Description)
                ModelState.AddModelError("Description","Name should be different from Description");
            //check validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var id = CityMock.Current.Cities.SelectMany(p=>p.PointsOfInterest).Max(x=>x.Id);
            var point = new PointsOfInterest
            {
                Id = ++id,
                Name = createPointsOfInterest.Name,
                Description = createPointsOfInterest.Description
            };
            city.PointsOfInterest.Add(point);
            return CreatedAtRoute("pointOfInterest",new {cityId=cityId,id=point.Id},point);
        }
        //fully update pointOfInterest
        [HttpPut("{cityId}/pointsOfInterest/{id}")]
        public IActionResult FullyUpdate(int cityId,int id,[FromBody]CreateUpdatePointsOfInterest createUpdatePointsOfInterest)
        {
            if (createUpdatePointsOfInterest == null)
                return BadRequest();
            if (createUpdatePointsOfInterest.Name == createUpdatePointsOfInterest.Description)
                ModelState.AddModelError("Description", "Name should be different from Description");
            //check validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
                return NotFound();
            pointOfInterest.Name = createUpdatePointsOfInterest.Name;
            pointOfInterest.Description = createUpdatePointsOfInterest.Description;
            return NoContent();
        }
        //partially update
        [HttpPatch("{cityId}/pointsOfInterest/{id}")]
        public IActionResult PartiallyUpdate(int cityId,int id,[FromBody] JsonPatchDocument<CreateUpdatePointsOfInterest> jsonPatch)
        {
            if (jsonPatch == null)
                return BadRequest();
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
                return NotFound();
            var pointOfInterestPatch = new CreateUpdatePointsOfInterest { Name = pointOfInterest.Name, Description = pointOfInterest.Description };
            jsonPatch.ApplyTo(pointOfInterestPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (pointOfInterestPatch.Name == pointOfInterestPatch.Description)
                ModelState.AddModelError("Description", "Name should be different from Description");
            TryValidateModel(pointOfInterestPatch);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            pointOfInterest.Name = pointOfInterestPatch.Name;
            pointOfInterest.Description = pointOfInterestPatch.Description;
            return NoContent();
        }
        //Delete ointOfInterest
        [HttpDelete("{cityId}/pointsOfInterest/{id}")]
        public IActionResult Delete(int cityId, int id)
        {
            var city = CityMock.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterest == null)
                return NotFound();
            city.PointsOfInterest.Remove(pointOfInterest);
            mail.Send("delete",$"Point of name = {pointOfInterest.Name} deleted");
            return NoContent();
        }
    }
}
