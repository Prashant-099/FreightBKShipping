using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("paytype")]
    public class PayType
    {
        [Key]
        [Column("paytype_id")]
        public int PayTypeId { get; set; }

        [Column("paytype_company_id")]

        public int PayTypeCompanyId { get; set; } 

        [Column("paytype_addedby_user_id")]

        public string? PayTypeAddedByUserId { get; set; }

        [Column("paytype_updatedby_user_id")]
  
        public string? PayTypeUpdatedByUserId { get; set; }

        [Column("paytype_name")]
    
        public string PayTypeName { get; set; } = string.Empty;

        [Column("paytype_status")]
        public bool PayTypeStatus { get; set; } = true;

        [Column("paytype_created")]
        public DateTime PayTypeCreated { get; set; } = DateTime.UtcNow;

        [Column("paytype_updated")]
        public DateTime? PayTypeUpdated { get; set; }
    }
}
