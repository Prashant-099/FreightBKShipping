using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{

    [Table("lr_details")]
    public class LRDetail
    {
        [Key]
        [Column("lr_details_id")]
        public int LrDetailsId { get; set; }

        [Required]
        [Column("lr_details_lr_id")]
        public int LrDetailsLrId { get; set; }

        [Column("lr_detail_sr_no")]
        public int? LrDetailSrNo { get; set; }

        [Column("lr_detail_product_id")]
        public int? LrDetailProductId { get; set; }

        [Column("lr_detail_unit")]
        [StringLength(50)]
        public string? LrDetailUnit { get; set; }

        [Column("lr_detail_gross_wt", TypeName = "decimal(12,3)")]
        public decimal? LrDetailGrossWt { get; set; }

        [Column("lr_detail_tare_wt", TypeName = "decimal(12,3)")]
        public decimal? LrDetailTareWt { get; set; }

        [Column("lr_detail_net_wt", TypeName = "decimal(12,3)")]
        public decimal? LrDetailNetWt { get; set; }

        [Column("lr_detail_load_wt", TypeName = "decimal(12,3)")]
        public decimal? LrDetailLoadWt { get; set; }

        [Column("lr_detail_unload_wt", TypeName = "decimal(12,3)")]
        public decimal? LrDetailUnloadWt { get; set; }

        // ✅ FIXED (was lr_detail_qty)
        [Column("lr_detail_chargeqty", TypeName = "decimal(12,3)")]
        public decimal? LrDetailChargeQty { get; set; }

        // ✅ FIXED (was lr_detail_rate)
        [Column("lr_detail_billrate", TypeName = "decimal(12,2)")]
        public decimal? LrDetailBillRate { get; set; }

        [Column("lr_detail_bill_type")]
        [StringLength(100)]
        public string? LrDetailBillType { get; set; }

        [Column("lr_detail_Grossfreight", TypeName = "decimal(14,2)")]
        public decimal? LrDetailGrossFreight { get; set; }

        [Column("lr_detail_total_bags")]
        public int? LrDetailTotalBags { get; set; }

        [Column("lr_detail_cargo_name")]
        [StringLength(255)]
        public string? LrDetailCargoName { get; set; }

        [Column("lr_detail_lot_no")]
        [StringLength(100)]
        public string? LrDetailLotNo { get; set; }

        [Column("lr_detail_grade")]
        [StringLength(100)]
        public string? LrDetailGrade { get; set; }

        [Column("lr_detail_calc")]
        [StringLength(50)]
        public string? LrDetailCalc { get; set; }

        [Column("lr_detail_amount", TypeName = "decimal(14,2)")]
        public decimal? LrDetailAmount { get; set; }

        [Column("lr_detail_invoice_no")]
        [StringLength(100)]
        public string? LrDetailInvoiceNo { get; set; }

        [Column("lr_detail_challan_no")]
        [StringLength(100)]
        public string? LrDetailChallanNo { get; set; }

        [Column("lr_detail_date")]
        public DateTime? LrDetailDate { get; set; }

        [Column("lr_detail_vehicle")]
        [StringLength(20)]
        public string? LrDetailVehicle { get; set; }

        [Column("lr_detail_driver")]
        [StringLength(45)]
        public string? LrDetailDriver { get; set; }

        [Column("lr_detail_del_address")]
        [StringLength(150)]
        public string? LrDetailDelAddress { get; set; }
    }
}
