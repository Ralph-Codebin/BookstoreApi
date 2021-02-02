using System;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace bookstore_api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    [ProducesResponseType(401)]
    public class ApiInfoController : ControllerBase
    {
         
        [HttpGet]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public ActionResult<APIInfo> Get()
        {
            Assembly[] myAssemblies = Thread.GetDomain().GetAssemblies();
            Assembly myAssembly = null;

            string versionnr = "";

            for (int i = 0; i < myAssemblies.Length; i++)
            {
                if (String.Compare(myAssemblies[i].GetName().Name, "bookstore_api") == 0)
                    myAssembly = myAssemblies[i];
            }
            if (myAssembly != null)
            {
                versionnr = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            APIInfo a = new APIInfo();
            a.APIname = "bookstore api API";
            a.Version = versionnr;
            a.PracticeData = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["PracticeDataDatabase"];
            a.Servicecatalog = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["ServiceCatalogDatabase"];

            return a;
        }
    }

    public class APIInfo
    {
        public string APIname { get; set; }
        public string Version { get; set; }
        public string PracticeData { get; set; }
        public string Servicecatalog { get; set; }
    }
}
