using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using bookstore_api.ModelsSC;
using bookstore_api.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace bookstore_api.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(401)]
    public class IncompleteSubscriptionController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<Subscriptions> ListAll()
        {
            SubscriptionsResponse bdr = new SubscriptionsResponse();
            
                using (var context = new DepContextSC())
                {
                    return context.Subscriptions.Where(sub => sub.Status != "active").ToList();
                }
            
        }

    }
}