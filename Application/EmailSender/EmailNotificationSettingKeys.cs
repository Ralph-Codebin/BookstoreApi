using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EmailSender
{
    public enum emailMsgTypes
    {
        newsubscription,
        updatesubscription,
        removeservice
    }

    public static class EmailNotificationSettingKeys
    {
        // NOTE These keys match to [ServiceCatalogue].[dbo].[NotificationSettings] ->[keyItem] 
        public const string NotificationTokenKey = "notificationtoken";
        public const string NotificationNameKey = "notificationname";
        public const string NotificationSenderNameKey = "notificationsendername";
        public const string NotificationEmailKey = "notificationemail";
        public const string NotificationSubjectKey = "notificationsubject";
        public const string newsubscriptionMessageKey = "newsubscriptionmsg";
        public const string updatesubscriptionMessageKey = "updatesubscriptionmsg";
        public const string removeserviceMessageKey = "removeservicemsg";
    }
}
