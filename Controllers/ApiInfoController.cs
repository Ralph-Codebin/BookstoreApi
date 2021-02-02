using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using bookstore_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace bookstore_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiInfoController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string db = config.GetSection("PracticeDataDatabase").Value;

            Log.Information("Contact made with our api.");
            return "bookstore api API v1.2 database: "+ db;
        }
    }
}
