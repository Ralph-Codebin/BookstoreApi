using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Entities
{
    public class MessageParameters
    {
        public string name { get; set; }
        public string body { get; set; }
        public string sender { get; set; }
    }
    public class MessagePayload
    {
        public string payload_type { get; set; }
        public string subject { get; set; }
        public string template { get; set; }
        public string parameters { get; set; }
    }

    public class MessageItem
    {
        public string channel { get; set; }
        public List<string> destination { get; set; }
        public string clientMessageId { get; set; }
        public MessagePayload message { get; set; }
    }

    public class NotificationObject
    {
        public List<MessageItem> messages { get; set; }
    }
}
