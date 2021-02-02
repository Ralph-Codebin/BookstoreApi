using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_api.Requests
{
    public class DNewSubscriptiontRequest
    {
        public int prodid { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string lasename { get; set; }
    }
}
