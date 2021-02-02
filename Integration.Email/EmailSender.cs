

using Application.EmailSender;
using Application.ErrorHandlers;
using Application.Repositories;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Domain.Model.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integration.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IRepositoryExceptionHandler _exceptionHandler;
        private readonly IConfiguration _config;
        public EmailSender( INotificationRepository notificationRepository, IConfiguration config, IRepositoryExceptionHandler exceptionHandler)
        {
            _notificationRepository = notificationRepository;
            _config = config;
            _exceptionHandler = exceptionHandler;
        }
             

        public async Task SendEmailNotification(emailMsgTypes msg, string recipientEmail, int retryCount, string recipientFullname, string fullSeviceName)
        {
            string body = $"";
            
            
            retryCount += 1;
            string notifcationChannel = "email";

            var settings = _notificationRepository.GetNotificationSettings();           
            var notificationEndpoint = _config.GetValue<string>("Integration:EmailSender:NotificationUrl");
            var maxRetryAttempts = _config.GetValue<int>("EmailNotificationSettings:MaxRetryCount");
            var validationErrorResponseKey = _config.GetValue<string>("EmailNotificationSettings:ValidationErrorResponseKey");

            string GetSetting(string key)
            {
                return settings[key].ToString();
            }

            switch (msg)
            {
                case emailMsgTypes.newsubscription:
                    body += $"{GetSetting(EmailNotificationSettingKeys.newsubscriptionMessageKey)} : {fullSeviceName} <br/>";
                    break;

                case emailMsgTypes.removeservice:
                    body += $"{GetSetting(EmailNotificationSettingKeys.removeserviceMessageKey)} : {fullSeviceName} <br/><br/>";
                    break;

                case emailMsgTypes.updatesubscription:
                    body+=  $"{GetSetting(EmailNotificationSettingKeys.updatesubscriptionMessageKey)} : {fullSeviceName} <br/><br/>";
                    break;
            }
  
            body += $" Please visit Practice Data on Sanport to ammend the information <br/> <br/> <br/>";
            body += $"Kind regrards <br/>";
            body += $"The bookstore api Team <br/>";
            body += $"This is an automated email. Please do not reply <br/>";

            var client = new RestClient(notificationEndpoint);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer " + GetSetting(EmailNotificationSettingKeys.NotificationTokenKey));

            NotificationObject notificationObject = new NotificationObject();
            List<MessageItem> messageItems = new List<MessageItem>();
            List<string> recipients = new List<string>();

            MessageItem messageItem = new MessageItem();
            MessagePayload messagePayload = new MessagePayload();
            MessageParameters mp = new MessageParameters();

            mp.name = $"{recipientFullname.ToLower()}";
            mp.body = body;
            mp.sender = GetSetting(EmailNotificationSettingKeys.NotificationSenderNameKey);

            messageItem.channel = notifcationChannel;

            if (recipientEmail == "")
            {
                recipients.Add(GetSetting(EmailNotificationSettingKeys.NotificationEmailKey)); 
            }
            else
            {
                // **** at some point this needs to be swapped
                recipients.Add(GetSetting(EmailNotificationSettingKeys.NotificationEmailKey));
                // recipients.Add(recipientEmail);
                // **** ------------------------
            }

			messageItem.destination = recipients;
			messageItem.clientMessageId = _config.GetValue<string>("EmailNotificationSettings:ClientMessageId");
            messagePayload.payload_type = _config.GetValue<string>("EmailNotificationSettings:PayloadType");
            messagePayload.template = _config.GetValue<string>("EmailNotificationSettings:Template");
            messagePayload.parameters = JsonConvert.SerializeObject(mp);
            messagePayload.subject = GetSetting(EmailNotificationSettingKeys.NotificationSubjectKey);

            messageItem.message = messagePayload;
            messageItems.Add(messageItem);

            notificationObject.messages = messageItems;

            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(notificationObject), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.Content.ToLower().Contains(validationErrorResponseKey))
            {
                _exceptionHandler.LogInformation("Could not retreive email token: "+ response.Content.ToString());
                GetNewNotificationToken();
                if (retryCount <= maxRetryAttempts)
                {
                    await SendEmailNotification(msg, recipientEmail, retryCount, recipientFullname, fullSeviceName);
                }
            }
        }

        private void GetNewNotificationToken()
        {
            var x = 0;
        }

    }
}
