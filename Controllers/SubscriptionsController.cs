using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using bookstore_api.ModelsSC;
using bookstore_api.Models;
using bookstore_api.Requests;
using bookstore_api.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics.CodeAnalysis;

namespace bookstore_api.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(401)]
    public class SubscriptionController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public object ListAll()
        {
            SubscriptionsResponse bdr = new SubscriptionsResponse();
            try
            {
                using (var context = new DepContextSC())
                {
                    var parameters = new[] {
                        new SqlParameter("@IntermediaryCode", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = "" }
                    };
                    bdr.subscriptiondata = context.SubscriptionsFull.FromSqlRaw<SubscriptionsFull>("SP_ListSubscriptions @IntermediaryCode", parameters).ToList();
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information("ListSubscriptionsError: " + e.Message);
                bdr.error = e.Message;
                //throw new Exception();
            }
            return bdr;
        }

        [HttpGet("{IntermediaryCode}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public object ListOne(string IntermediaryCode)
        {
            SubscriptionsResponse bdr = new SubscriptionsResponse();
            try
            {
                using (var context = new DepContextSC())
                {
                    var parameters = new[] {
                        new SqlParameter("@IntermediaryCode", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = IntermediaryCode }
                    };
                    bdr.subscriptiondata = context.SubscriptionsFull.FromSqlRaw<SubscriptionsFull>("SP_ListSubscriptions @IntermediaryCode", parameters).ToList();
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information("ListSubscriptionsError: " + e.Message);
                bdr.error = e.Message;
                //throw new Exception();
            }
            return bdr;
        }

        [HttpPatch("{SubscriptionID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public object UpdateSubscriptions(int SubscriptionID, [FromBody] SubscriptionsRequest bus)
        {
            SubscriptionsResponse bdr = new SubscriptionsResponse();
            try
            {
                using (var context = new DepContextSC())
                {
                    string tokenforCall = "";
                    Subscriptions bd = context.Subscriptions.Where(bus => bus.Id == SubscriptionID).FirstOrDefault();
                    bd.Serviceid = bus.Serviceid;
                    bd.IntermediaryCode = bus.IntermediaryCode;
                    bd.Status = bus.Status;
                    context.SaveChanges();

                    if (bd.Status.ToLower() != "active") {
                        if (context.TempCache.Any(o => o.keyitem == "notificationtoken"))
                        {
                            TempCache scr = context.TempCache.Where(bus => bus.keyitem == "notificationtoken").FirstOrDefault();
                            tokenforCall = scr.valueitem;
                        }
                        else
                        {
                            tokenforCall = GetNewNotificationToken();
                        };
                        string msg = "A subscription status was changed -  IntermediaryCode: " + bd.IntermediaryCode + " with ServiceID: " + bd.Serviceid.ToString();

                        if (SendEmailNotification(tokenforCall, msg, "Subscription cancellation") == "valadationfail")
                        {
                            tokenforCall = GetNewNotificationToken();

                            SendEmailNotification(tokenforCall, msg, "Subscription cancellation");
                        };
                    }

                    bdr.subscriptiondata = context.Subscriptions.Where(bus => bus.Id == SubscriptionID).ToList();
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information("UpdateSubscriptionsError: " + e.Message);
                bdr.error = e.Message;
                // throw new Exception();
            }
            return bdr;
        }

        [HttpPost("{IntermediaryCode}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IEnumerable<Subscriptions> NewSubscription(string IntermediaryCode, NewSubscriptionRequest bus)
        {
            using (var context = new DepContextSC())
            {
                string tokenforCall = "";
                Subscriptions sub = new Subscriptions();
                sub.IntermediaryCode = IntermediaryCode;
                sub.Serviceid = bus.Serviceid;
                sub.Status = bus.Status;

                context.Subscriptions.Add(sub);
                context.SaveChanges();

                if (context.TempCache.Any(o => o.keyitem == "notificationtoken"))
                {
                    TempCache bd = context.TempCache.Where(bus => bus.keyitem == "notificationtoken").FirstOrDefault();
                    tokenforCall = bd.valueitem;
                }
                else
                {
                    tokenforCall = GetNewNotificationToken();
                };
                string msg = "A new subscription was created IntermediaryCode: " + sub.IntermediaryCode + " with ServiceID: " + sub.Serviceid.ToString();

                if (SendEmailNotification(tokenforCall, msg, "New Subscription") == "valadationfail")
                {
                    tokenforCall = GetNewNotificationToken();

                    SendEmailNotification(tokenforCall, msg, "New Subscription");
                };

                return context.Subscriptions.Where(bus => bus.IntermediaryCode == IntermediaryCode).ToList();
            }
        }

        private string GetNewNotificationToken()
        {
            var Authclient = new RestClient("https://api-dev.sanlam.co.za/auth/oauth/v2/token");
            var Authrequest = new RestRequest(Method.POST);
            Authrequest.AddHeader("content-type", "application/x-www-form-urlencoded");
            Authrequest.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=l7401ccaea01034052abbda3d281b08d60&client_secret=0268a61963ca410a8181e1dc790c45e0", ParameterType.RequestBody);
            IRestResponse Authresponse = Authclient.Execute(Authrequest);
            TokenModel token = new TokenModel(Authresponse.Content);
            using (var tokencontext = new DepContextSC())
            {

                if (tokencontext.TempCache.Any(o => o.keyitem == "notificationtoken"))
                {
                    var updatetoken = tokencontext.TempCache.FirstOrDefault(e => e.keyitem == "notificationtoken");
                    updatetoken.valueitem = token.access_token;
                }
                else {
                    TempCache tx = new TempCache();
                    tx.keyitem = "notificationtoken";
                    tx.valueitem = token.access_token;
                    tokencontext.TempCache.Add(tx);
                };
                tokencontext.SaveChanges();                    

            }

            return token.access_token;
        }

        
        private string SendEmailNotification(string tokenforCall, string comment, string subject )
        {
            var client = new RestClient("https://api-dev.sanlam.co.za/gti/communication-gateway/v1/message");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer " + tokenforCall);

            NotificationObject notificationObject = new NotificationObject();
            List<MessageItem> messageItems = new List<MessageItem>();
            List<string> recipients = new List<string>();

            MessageItem messageItem = new MessageItem();
            MessagePayload messagePayload = new MessagePayload();

            messageItem.channel = "email";
            recipients.Add("shawn.coetzer@sanlam.co.za");
            recipients.Add("ralph.kleinschmidt@sanlam.co.za");
            messageItem.destination = recipients;
            messageItem.clientMessageId = "0000";

            messagePayload.payload_type = "text";
            messagePayload.subject =subject;
            messagePayload.content = comment;

            messageItem.message = messagePayload;
            messageItems.Add(messageItem);

            notificationObject.messages = messageItems;

            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(notificationObject), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.Content.ToLower().Contains("validation error"))
            {
                return "valadationfail";
            }
            else {
                return "notification sent.";
            }
        }
    }
}