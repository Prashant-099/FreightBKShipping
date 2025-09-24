using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("units")]
    public class Unit
    {
        [Key]
        [Column("unit_id")]
        public int UnitId { get; set; }

        [Column("unit_company_id")]
      
        public int UnitCompanyId { get; set; }

        [Column("unit_addedby_user_id")]
        
        public string? UnitAddedByUserId { get; set; }

        [Column("unit_updatedby_user_id")]
       
        public string? UnitUpdatedByUserId { get; set; }

        [Column("unit_name")]
       
        public string UnitName { get; set; } = string.Empty;

        [Column("unit_formalname")]
      
        public string? UnitFormalName { get; set; }

        [Column("unit_gstunit")]
     
        public string? UnitGstUnit { get; set; }

        [Column("unit_status")]
        public bool UnitStatus { get; set; } = true;

        [Column("unit_created")]
        public DateTime UnitCreated { get; set; } = DateTime.UtcNow;

        [Column("unit_updated")]
        public DateTime? UnitUpdated { get; set; }
    }
}
