using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model.Entities
{
    public class DNewProductRequest
    {
        public string imagePath { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string price { get; set; }
    }
}
