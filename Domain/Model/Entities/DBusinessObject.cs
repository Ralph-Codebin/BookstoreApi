using Domain.Model.ResponseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model.Entities
{
    [NotMapped]
    public class DBusinessObject
    {
        public object productdata { get; set; }
        public string error { get; set; }
    }
}
