using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EntityFramework.Models
{
    public class ControllData
    {
        public int? id { get; set; }
        public int? dbId { get; set; }
        public string fieldname { get; set; }
        public string description { get; set; }
        public string validation { get; set; }
        public string tagname { get; set; }
        public string placement { get; set; }
        public string errormessage { get; set; }
        public string helpmessage { get; set; }
        public int controltype { get; set; }
        public string typeids { get; set; }
        public string value { get; set; }
        public bool isreadonly { get; set; }
        public string intermediarycode { get; set; }
        public bool isinlineradio { get; set; }
        public string maxsize { get; set; }
        public bool storeindep { get; set; }
    }
}
