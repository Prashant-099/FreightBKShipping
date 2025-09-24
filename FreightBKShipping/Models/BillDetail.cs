using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("bill_details")]
    public class BillDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("bill_detail_id")]
        public int BillDetailId { get; set; }

        [Column("bill_detail_addedby_user_id")]
        [StringLength(30)]
        public string? BillDetailAddedByUserId { get; set; }

        [Column("bill_detail_updatedby_user_id")]
        [StringLength(30)]
        public string? BillDetailUpdatedByUserId { get; set; }

        [Column("bill_detail_bill_id")]
        public int BillDetailBillId { get; set; }

        [Column("bill_detail_product_id")]
        public int BillDetailProductId { get; set; }

        [Column("bill_detail_unit_id")]
        public int BillDetailUnitId { get; set; }

        [Column("bill_detail_hsnid")]
        public int BillDetailHsnId { get; set; }

        [Column("bill_detail_qty")]
        public double BillDetailQty { get; set; }

        [Column("bill_detail_billqty")]
        public double BillDetailBillQty { get; set; }

        [Column("bill_detail_rate")]
        public double BillDetailRate { get; set; }

        [Column("bill_detail_actualrate")]
        public double BillDetailActualRate { get; set; }

        [Column("bill_detail_amount")]
        public double BillDetailAmount { get; set; }

        [Column("bill_detail_currency_id")]
        public int BillDetailCurrencyId { get; set; }

        [Column("bill_detail_exchrate")]
        public double BillDetailExchRate { get; set; }

        [Column("bill_detail_exchamount")]
        public double BillDetailExchAmount { get; set; }

        [Column("bill_detail_disper1")]
        public double BillDetailDisPer1 { get; set; }

        [Column("bill_detail_disamt1")]
        public double BillDetailDisAmt1 { get; set; }

        [Column("bill_detail_gstdisper")]
        public double BillDetailGstDisPer { get; set; }

        [Column("bill_detail_gstdisamt")]
        public double BillDetailGstDisAmt { get; set; }

        [Column("bill_detail_distotal")]
        public double BillDetailDisTotal { get; set; }

        [Column("bill_detail_taxableamt")]
        public double BillDetailTaxableAmt { get; set; }

        [Column("bill_detail_igstper")]
        public float BillDetailIgstPer { get; set; }

        [Column("bill_detail_sgstper")]
        public float BillDetailSgstPer { get; set; }

        [Column("bill_detail_cgstper")]
        public float BillDetailCgstPer { get; set; }

        [Column("bill_detail_igst")]
        public double BillDetailIgst { get; set; }

        [Column("bill_detail_sgst")]
        public double BillDetailSgst { get; set; }

        [Column("bill_detail_cgst")]
        public double BillDetailCgst { get; set; }

        [Column("bill_detail_cessper")]
        public double BillDetailCessPer { get; set; }

        [Column("bill_detail_cessamt")]
        public double BillDetailCessAmt { get; set; }

        [Column("bill_detail_addcessper")]
        public double BillDetailAddCessPer { get; set; }

        [Column("billp_addcessamt")]
        public double BillPAddCessAmt { get; set; }

        [Column("bill_detail_roundoff")]
        public double BillDetailRoundOff { get; set; }

        [Column("bill_detail_total")]
        public double BillDetailTotal { get; set; }

        [Column("bill_detail_unitcost")]
        public double BillDetailUnitCost { get; set; }

        [Column("bill_detail_remarks")]
        [StringLength(500)]
        public string? BillDetailRemarks { get; set; }

        [Column("bill_detail_slab_id")]
        [StringLength(30)]
        public string? BillDetailSlabId { get; set; }

        [Column("bill_detail_sno")]
        [StringLength(30)]
        public string? BillDetailSno { get; set; }

        [Column("bill_detail_batchno")]
        [StringLength(100)]
        public string? BillDetailBatchNo { get; set; }

        [Column("bill_detail_km")]
        [StringLength(75)]
        public string? BillDetailKm { get; set; }

        [Column("bill_detail_expirydt")]
        [StringLength(45)]
        public string? BillDetailExpiryDt { get; set; }

        [Column("bill_detail_serial1")]
        [StringLength(75)]
        public string? BillDetailSerial1 { get; set; }

        [Column("bill_detail_serial2")]
        [StringLength(75)]
        public string? BillDetailSerial2 { get; set; }

        [Column("bill_detail_account_id")]
        public int BillDetailAccountId { get; set; }

        [Column("bill_detail_igstac_id")]
        public int BillDetailIgstAcId { get; set; }

        [Column("bill_detail_sgstac_id")]
        public int BillDetailSgstAcId { get; set; }

        [Column("bill_detail_cgstac_id")]
        public int BillDetailCgstAcId { get; set; }

        [Column("bill_detail_cessac_id")]
        public int BillDetailCessAcId { get; set; }

        [Column("bill_detail_addcessac_id")]
        public int BillDetailAddCessAcId { get; set; }

        [Column("bill_detail_status")]
        public bool BillDetailStatus { get; set; }

        [Column("bill_detail_created")]
        public DateTime BillDetailCreated { get; set; } = DateTime.UtcNow;

        [Column("bill_detail_updated")]
        public DateTime? BillDetailUpdated { get; set; }

        [Column("bill_detail_hsncode")]
        [StringLength(15)]
        public string? BillDetailHsnCode { get; set; }

        [Column("bill_detail_unit")]
        [StringLength(4)]
        public string? BillDetailUnit { get; set; }

        [Column("bill_detail_exchunit")]
        [StringLength(15)]
        public string? BillDetailExchUnit { get; set; }

        [Column("bill_detail_extrachrg")]
        public float BillDetailExtraChrg { get; set; }

        [Column("bill_detail_vehicle_id")]
        public int BillDetailVehicleId { get; set; }

        [Column("bill_detail_gstper")]
        public float BillDetailGstPer { get; set; }
        [ForeignKey(nameof(BillDetailBillId))]
        public Bill Bill { get; set; }
    }
}
