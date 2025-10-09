using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("vessels")]
    public class Vessel
    {
        [Key]
        [Column("vessel_id")]
        public int VesselId { get; set; }

        [Column("vessel_uuid")]
    
        public string? VesselUuid { get; set; }

        [Column("vessel_company_id")]
        public int VesselCompanyId { get; set; }

        [Column("vessel_addedby_user_id")]
        
        public string? VesselAddedByUserId { get; set; }

        [Column("vessel_updatedby_user_id")]
        
        public string? VesselUpdatedByUserId { get; set; }

        [Column("vessel_name")]
        
        public string VesselName { get; set; } = string.Empty;

        [Column("vessel_status")]
        public bool VesselStatus { get; set; } = true;

        [Column("vessel_created")]
        public DateTime VesselCreated { get; set; } = DateTime.UtcNow;

        [Column("vessel_updated")]
        public DateTime? VesselUpdated { get; set; }

        [Column("vessel_qty")]
        public double? VesselQty { get; set; }

        [Column("vessel_cbm")]
        public double? VesselCbm { get; set; }

        [Column("vessel_dtsailing")]
        public DateTime? VesselDtSailing { get; set; }

        [Column("vessel_dtberting")]
        public DateTime? VesselDtBerting { get; set; }

        [Column("vessel_dtdemmurate")]
        public DateTime? VesselDtDemmurate { get; set; }

        [Column("vessel_noofbl")]
        public int? VesselNoOfBL { get; set; }

        [Column("vessel_qty_opening")]
        public double? VesselQtyOpening { get; set; }

        [Column("vessel_cbm_opening")]
        public double? VesselCbmOpening { get; set; }

        [Column("vessel_noofbl_opening")]
        public double? VesselNoOfBLOpening { get; set; }

        [Column("vesselscol")]
       
        public string? VesselsCol { get; set; }

        [Column("vessel_eta")]
        public DateTime? VesselEta { get; set; }

        [Column("vessel_edt")]
        public DateTime? VesselEdt { get; set; }

        [Column("vessel_ata")]
        public DateTime? VesselAta { get; set; }
    }
}
