using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("Status")]
    public class Status
    {
        [Key]
        [Column("Status_id")]
        public int StatusId { get; set; }

        [Column("Status_name")]
        public string StatusName { get; set; }

        [Column("Status_created")]
        public DateTime StatusCreated { get; set; }

        [Column("Status_updated")]
        public DateTime StatusUpdated { get; set; }

        [Column("Status_createdbyuser")]
        public string StatusCreatedByUser { get; set; }

        [Column("Status_updatedbyuser")]
        public string StatusUpdatedByUser { get; set; }

        [Column("Status_code")]
        public string Status_code { get; set; }
    }
}
