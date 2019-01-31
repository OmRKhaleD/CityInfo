using CityInfo.Api.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Controllers
{
    public class TestDatabaseController : Controller
    {
        CityDbContext CityDbContext;
        public TestDatabaseController(CityDbContext cityDbContext)
        {
            CityDbContext = cityDbContext;
        }
        [HttpGet]
        [Route("api/testdatabse")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
