using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model.Entities
{
    public enum  status
    {
        error,
        success,
        invalidinput
    }

    [NotMapped]
    public class DReturnObject
    {

        public object data { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
}
