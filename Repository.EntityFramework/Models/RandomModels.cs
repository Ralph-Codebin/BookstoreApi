using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EntityFramework.Models
{
    public class Datum
    {
        public string key { get; set; }
        public string value { get; set; }
        public string dataid { get; set; }
    }

    public class MyArray
    {
        public string tagitem { get; set; }
        public List<Datum> data { get; set; }
    }
}
