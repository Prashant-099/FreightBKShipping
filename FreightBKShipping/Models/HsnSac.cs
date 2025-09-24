using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("hsnsac")]
    public class HsnSac
    {
        [Key]
        [Column("hsn_id")]
        public int HsnId { get; set; }

        [Column("hsn_uuid")]
        
        public string HsnUuid { get; set; } = Guid.NewGuid().ToString("N");

        [Column("hsn_company_id")]
        public int HsnCompanyId { get; set; }

        [Column("hsn_addedby_user_id")]
       
        public string? HsnAddedByUserId { get; set; }

        [Column("hsn_updatedby_user_id")]
       
        public string? HsnUpdatedByUserId { get; set; }

        [Column("hsn_name")]
       
        public string HsnName { get; set; } = string.Empty;

        [Column("hsn_gstper")]
        public float HsnGstPer { get; set; }

        [Column("hsn_cesstype")]
       
        public string? HsnCessType { get; set; }

        [Column("hsn_cess")]
        public float? HsnCess { get; set; }

        [Column("hsn_addcess")]
        public float? HsnAddCess { get; set; }

        [Column("hsn_gstslab_id")]
        
        public int? HsnGstSlabId { get; set; }

        [Column("hsn_created")]
        public DateTime HsnCreated { get; set; } = DateTime.UtcNow;

        [Column("hsn_updated")]
        public DateTime? HsnUpdated { get; set; }

        [Column("hsn_status")]
        public bool HsnStatus { get; set; } = true;
    }
}
