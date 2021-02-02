using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EntityFramework.Models
{
    public class JsonReturnObject
    {
        public object BusinessData { get; set; }
        public object StaffData { get; set; }
        public object PersonalData { get; set; }
    }
}
