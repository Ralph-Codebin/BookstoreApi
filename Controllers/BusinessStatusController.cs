using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using bookstore_api.Models;
using bookstore_api.Requests;
using bookstore_api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace bookstore_api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(401)]
    public class BusinessStatusController : ControllerBase
    {
        [HttpPatch("{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<BusinessData> UpdateBusiness(Int32 Id, [FromBody] BusinessStatusRequest bus)
        {
            try
            {
                using (var context = new DepContext())
                {
                    BusinessData bd = context.BusinessData.Where(bus => bus.Id == Id).FirstOrDefault();
                    bd.Status = bus.Status;
                    context.SaveChanges();

                    return context.BusinessData.Where(bus => bus.Id == Id).ToList();
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information("UpdateBusinessStatusError: " + e.Message);
                throw new Exception();
            }
        }

    }
}
