using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore_api.Requests
{
    public class NewProductRequest
    {
        public string imagePath { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string price { get; set; }
    }
}
