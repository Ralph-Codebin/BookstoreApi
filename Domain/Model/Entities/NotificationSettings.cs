using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model.Entities
{
    public partial class NotificationSettings
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("keyitem")]
        public string keyitem { get; set; }
        [Column("valueitem")]
        public string valueitem { get; set; }
    }
}
