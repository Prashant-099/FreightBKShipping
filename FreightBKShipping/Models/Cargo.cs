using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("cargoes")]
    public class Cargo
    {
        [Key]
        [Column("cargo_id")]
        public int CargoId { get; set; }

        [Column("cargo_company_id")]
        public int CargoCompanyId { get; set; }

        [Column("cargo_addby_user_id")]
     
        public string? CargoAddbyUserId { get; set; }

        [Column("cargo_updatedby_user_id")]
       
        public string? CargoUpdatedbyUserId { get; set; }

        [Column("cargo_name")]

        public string CargoName { get; set; } = string.Empty;

        [Column("cargo_type")]
      
        public string? CargoType { get; set; }

        [Column("cargo_remarks")]
       
        public string? CargoRemarks { get; set; }

        [Column("cargo_hsn")]
        public int? CargoHsn { get; set; }

        [Column("cargo_status")]
        public bool CargoStatus { get; set; } = true;

        [Column("cargo_created")]
        public DateTime CargoCreated { get; set; } = DateTime.UtcNow;

        [Column("cargo_updated")]
        public DateTime? CargoUpdated { get; set; }

        [Column("cargo_gstper")]
        public float CargoGstPer { get; set; }

        [Column("cargo_cess")]
        public float CargoCess { get; set; }
    }
}
