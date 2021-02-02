using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_api.Models
{
    public class ReturnObject
    {

        public object data { get; set; }
        public string status { get; set; }
        public string message { get; set; }

    }
}