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
    public class BusinessDataController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<BusinessData> ListAll()
        {
            try
            {
                using (var context = new DepContext())
                {
                    return context.BusinessData.ToList();
                }
            }
            catch (Exception e)
            {
                List<BusinessData> list = new List<BusinessData>();
                BusinessData bd = new BusinessData();
                bd.FacebookUrl = e.Message;
                list.Add(bd);
                return list;
            }
        }

        [HttpGet("{IntermediaryCode}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public object ListOne(string IntermediaryCode)
        {
            BusinessDataResponse bdr = new BusinessDataResponse();
            try
            {
                using (var context = new DepContext())
                {
                    bdr.businessdata = context.BusinessData.Where(bus => bus.IntermediaryCode == IntermediaryCode).ToList();
                    bdr.prefferencedata = context.PreferenceData.Where(bus => bus.IntermediaryCode == IntermediaryCode).ToList();

                    List<BusinessDataToRandom> mylist = new List<BusinessDataToRandom>(context.BusinessDataToRandom.Where(bus => bus.IntermediaryCode == IntermediaryCode).ToList());
                    Hashtable prefs = new Hashtable();
                    foreach (var item in mylist)
                    {
                        if (prefs.ContainsKey(item.Linktable.ToLower()))
                        {
                            prefs[item.Linktable.ToLower()] = prefs[item.Linktable.ToLower()].ToString() + ',' + item.Tblid;
                        }
                        else
                        {
                            prefs.Add(item.Linktable.ToLower(), item.Tblid);
                        }
                    }
                    if (prefs.ContainsKey("languages"))
                    {
                        int[] ids = prefs["languages"].ToString().Split(',').Select(int.Parse).ToArray();
                        bdr.languagedata = context.Languages.Where(bus => ids.Contains(bus.Id)).ToList();
                    }
                    if (prefs.ContainsKey("marketingactivities"))
                    {
                        int[] ids = prefs["marketingactivities"].ToString().Split(',').Select(int.Parse).ToArray();
                        bdr.marketingactivities = context.MarketingActivities.Where(bus => ids.Contains(bus.Id)).ToList();
                    }
                    if (prefs.ContainsKey("marketingobjectives"))
                    {
                        int[] ids = prefs["marketingobjectives"].ToString().Split(',').Select(int.Parse).ToArray();
                        bdr.marketingobjectives = context.MarketingObjectives.Where(bus => ids.Contains(bus.Id)).ToList();
                    }
                    if (prefs.ContainsKey("marketinglocations"))
                    {
                        int[] ids = prefs["marketinglocations"].ToString().Split(',').Select(int.Parse).ToArray();
                        bdr.marketinglocations = context.MarketingLocations.Where(bus => ids.Contains(bus.Id)).ToList();
                    }
                    if (prefs.ContainsKey("serviceofferings"))
                    {
                        int[] ids = prefs["serviceofferings"].ToString().Split(',').Select(int.Parse).ToArray();
                        bdr.serviceofferings = context.ServiceOfferings.Where(bus => ids.Contains(bus.Id)).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information("ListBusinessDataError: " + e.Message);
                bdr.error = e.Message;
                //throw new Exception();
            }
                return bdr;
        }

        [HttpPatch("{IntermediaryCode}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<BusinessData> UpdateBusiness(string IntermediaryCode, [FromBody] BusinessDataRequest bus)
        {
            try { 
                using (var context = new DepContext())
                {
                    BusinessData bd = context.BusinessData.Where(bus => bus.IntermediaryCode == IntermediaryCode).FirstOrDefault();
                    bd.WebsiteUrl = bus.WebsiteUrl;
                    bd.GoogleBusinessUrl = bus.GoogleBusinessUrl;
                    bd.DigitalBcardUrl = bus.DigitalBcardUrl;
                    bd.WhatsappNumber = bus.WhatsappNumber;
                    bd.FacebookUrl = bus.FacebookUrl;
                    bd.LinkedinUrl = bus.LinkedinUrl;
                    bd.DirectoryListingUrl = bus.DirectoryListingUrl;                   
                    context.SaveChanges();

                    return context.BusinessData.Where(bus => bus.IntermediaryCode == IntermediaryCode).ToList();
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information("UpdateBusinessError: " + e.Message);
                throw new Exception();
            }
        }

        // ********** this is for later use
        //[HttpPost]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(200)]
        //public IEnumerable<BusinessData> NewBusiness(BusinessData bus)
        //{
        //    using (var context = new DepContext())
        //    {
        //        BusinessData bd = new BusinessData();
        //        bd.IntermediaryCode = bus.IntermediaryCode;
        //        bd.WebsiteUrl = bus.WebsiteUrl;
        //        bd.GoogleBusinessUrl = bus.GoogleBusinessUrl;
        //        bd.DigitalBcardUrl = bus.DigitalBcardUrl;
        //        bd.WhatsappNumber = bus.WhatsappNumber;
        //        bd.FacebookUrl = bus.FacebookUrl;
        //        bd.LinkedinUrl = bus.LinkedinUrl;
        //        bd.DirectoryListingUrl = bus.DigitalBcardUrl;

        //        context.BusinessData.Add(bd);
        //        context.SaveChanges();

        //        return context.BusinessData.Where(bus => bus.IntermediaryCode == bus.IntermediaryCode).ToList();
        //    }
        //}
    }
}
