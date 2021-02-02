using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookstore_api.ModelsSC;
using bookstore_api.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace bookstore_api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(401)]
    public class ServiceCatelogController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<Services> ListAll()
        {
            using (var context = new DepContextSC())
            {
                return context.Services.ToList();
            }
        }

        [HttpGet("{CategoryID}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<Services> ListAllForThisCategory(int CategoryID)
        {
            using (var context = new DepContextSC())
            {
                return context.Services.Where(bus => bus.Categoryid == CategoryID).ToList();
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<Services> UpdateServices(int id, Services bus)
        {
            try { 
                using (var context = new DepContextSC())
                {
                    Services bd = context.Services.Where(bus => bus.Id == id).FirstOrDefault();
                    bd.Categoryid = bus.Categoryid;
                    bd.Name = bus.Name;
                    bd.Description = bus.Description;
                    bd.Cost = bus.Cost;
                    bd.Terms = bus.Terms;
                    bd.Status = bus.Status;
                    context.SaveChanges();

                    return context.Services.Where(bus => bus.Id == id).ToList();
                }
            }
                catch (Exception e)
                {
                    Serilog.Log.Information("UpdateServicesError: " + e.Message);
                    throw new Exception();
            }
        }
    }
}
