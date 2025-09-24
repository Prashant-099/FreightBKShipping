using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("bills")]
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("bill_id")]
        public int BillId { get; set; }

        [Column("bill_company_id")]
        public int BillCompanyId { get; set; }

        [Column("bill_addedby_user_id")]
        [StringLength(90)]
        public string? BillAddedByUserId { get; set; }

        [Column("bill_updatedby_user_id")]
        [StringLength(90)]
        public string? BillUpdatedByUserId { get; set; }

        [Column("bill_partyid")]
        public int BillPartyId { get; set; }

        [Column("bill_voucher_id")]
        public int BillVoucherId { get; set; }

        [Column("bill_year_id")]
        public int BillYearId { get; set; }

        [Column("bill_salesman_id")]
        [StringLength(30)]
        public string? BillSalesmanId { get; set; }

        [Column("bill_no")]
        [StringLength(50)]
        public string? BillNo { get; set; }

        [Column("bill_vch_no")]
        public int BillVchNo { get; set; }

        [Column("bill_date")]
        public DateTime BillDate { get; set; }

        [Column("bill_time")]
        [StringLength(45)]
        public string? BillTime { get; set; }

        [Column("bill_duedate")]
        public DateTime? BillDueDate { get; set; }

        [Column("bill_type")]
        [StringLength(20)]
        public string? BillType { get; set; }

        [Column("bill_amount")]
        public double BillAmount { get; set; }

        [Column("bill_disper1")]
        public double BillDisPer1 { get; set; }

        [Column("bill_discount1")]
        public double BillDiscount1 { get; set; }

        [Column("bill_igst")]
        public double BillIgst { get; set; }

        [Column("bill_sgst")]
        public double BillSgst { get; set; }

        [Column("bill_cgst")]
        public double BillCgst { get; set; }

        [Column("bill_taxableamt")]
        public double BillTaxableAmt { get; set; }

        [Column("bill_nontaxable")]
        public double BillNonTaxable { get; set; }

        [Column("bill_roundamt")]
        public double BillRoundAmt { get; set; }

        [Column("bill_isroundoff")]
        public bool BillIsRoundOff { get; set; }

        [Column("bill_gstid_freight")]
        public int BillGstIdFreight { get; set; }

        [Column("bill_gstid_charge")]
        public int BillGstIdCharge { get; set; }

        [Column("bill_gstamt_freight")]
        public double BillGstAmtFreight { get; set; }

        [Column("bill_gstamt_charge")]
        public double BillGstAmtCharge { get; set; }

        [Column("bill_total_usd")]
        public double BillTotalUsd { get; set; }

        [Column("bill_total")]
        public double BillTotal { get; set; }

        [Column("bill_placeofsupply")]
        public int BillPlaceOfSupply { get; set; }

        [Column("bill_supply_type")]
        [StringLength(30)]
        public string? BillSupplyType { get; set; }

        [Column("bill_ship_party")]
        [StringLength(150)]
        public string? BillShipParty { get; set; }

        [Column("bill_address1")]
        [StringLength(200)]
        public string? BillAddress1 { get; set; }

        [Column("bill_address2")]
        [StringLength(200)]
        public string? BillAddress2 { get; set; }

        [Column("bill_address3")]
        [StringLength(150)]
        public string? BillAddress3 { get; set; }

        [Column("bill_city")]
        [StringLength(45)]
        public string? BillCity { get; set; }

        [Column("bill_contactno")]
        [StringLength(45)]
        public string? BillContactNo { get; set; }

        [Column("bill_gstno")]
        [StringLength(20)]
        public string? BillGstNo { get; set; }

        [Column("bill_stateid")]
        public int BillStateId { get; set; }

        [Column("bill_against_billdate")]
        [StringLength(45)]
        public string? BillAgainstBillDate { get; set; }

        [Column("bill_against_billno")]
        [StringLength(45)]
        public string? BillAgainstBillNo { get; set; }

        [Column("bill_drcr")]
        [StringLength(20)]
        public string? BillDrCr { get; set; }

        [Column("bill_iscancel")]
        public bool BillIsCancel { get; set; }

        [Column("bill_isfreeze")]
        public bool BillIsFreeze { get; set; }

        [Column("bill_taxIncluded")]
        public bool BillTaxIncluded { get; set; }

        [Column("bill_by")]
        [StringLength(45)]
        public string? BillBy { get; set; }

        [Column("bill_remarks")]
        [StringLength(255)]
        public string? BillRemarks { get; set; }

        [Column("bill_AmountInword")]
        [StringLength(255)]
        public string? BillAmountInWord { get; set; }

        [Column("bill_jobno")]
        [StringLength(45)]
        public string? BillJobNo { get; set; }

        [Column("bill_job_type")]
        [StringLength(45)]
        public string? BillJobType { get; set; }

        [Column("bill_pod_id")]
        public int BillPodId { get; set; }

        [Column("bill_pol_id")]
        public int BillPolId { get; set; }

        [Column("bill_vessel_id")]
        public int BillVesselId { get; set; }

        [Column("bill_line_id")]
        public int BillLineId { get; set; }

        [Column("bill_cargo_id")]
        public int BillCargoId { get; set; }

        [Column("bill_consignee_id")]
        public int BillConsigneeId { get; set; }

        [Column("bill_shipper_id")]
        public int BillShipperId { get; set; }

        [Column("bill_sbno")]
        [StringLength(70)]
        public string? BillSbNo { get; set; }

        [Column("bill_sbdate")]
        public DateTime? BillSbDate { get; set; }

        [Column("bill_blno")]
        [StringLength(70)]
        public string? BillBlNo { get; set; }

        [Column("bill_bldate")]
        public DateTime? BillBlDate { get; set; }

        [Column("bill_shipper_inv_no")]
        [StringLength(70)]
        public string? BillShipperInvNo { get; set; }

        [Column("bill_shipper_inv_date")]
        public DateTime? BillShipperInvDate { get; set; }

        [Column("bill_grosswt")]
        public double BillGrossWt { get; set; }

        [Column("bill_netwt")]
        public double BillNetWt { get; set; }

        [Column("bill_qty")]
        public double BillQty { get; set; }

        [Column("bill_exchrate")]
        public double BillExchRate { get; set; }

        [Column("bill_20ft")]
        [StringLength(45)]
        public string? Bill20Ft { get; set; }

        [Column("bill_40ft")]
        [StringLength(45)]
        public string? Bill40Ft { get; set; }

        [Column("bill_container_no")]
        [StringLength(500)]
        public string? BillContainerNo { get; set; }

        [Column("bill_cust1")]
        [StringLength(60)]
        public string? BillCust1 { get; set; }

        [Column("bill_cust2")]
        [StringLength(60)]
        public string? BillCust2 { get; set; }

        [Column("bill_cust3")]
        [StringLength(60)]
        public string? BillCust3 { get; set; }

        [Column("bill_cust4")]
        [StringLength(60)]
        public string? BillCust4 { get; set; }

        [Column("bill_cust5")]
        [StringLength(60)]
        public string? BillCust5 { get; set; }

        [Column("bill_cust6")]
        [StringLength(60)]
        public string? BillCust6 { get; set; }

        [Column("bill_irn_no")]
        [StringLength(200)]
        public string? BillIrnNo { get; set; }

        [Column("bill_ack_no")]
        [StringLength(60)]
        public string? BillAckNo { get; set; }

        [Column("bill_ack_date")]
        [StringLength(20)]
        public string? BillAckDate { get; set; }

        [Column("bill_status")]
        public bool BillStatus { get; set; }

        [Column("bill_created")]
        public DateTime BillCreated { get; set; } = DateTime.UtcNow;

        [Column("bill_updated")]
        public DateTime? BillUpdated { get; set; }

        [Column("bill_prefix")]
        [StringLength(20)]
        public string? BillPrefix { get; set; }

        [Column("bill_postfix")]
        [StringLength(20)]
        public string? BillPostfix { get; set; }

        [Column("bill_default_currency_id")]
        public int BillDefaultCurrencyId { get; set; }

        [Column("bill_group")]
        [StringLength(45)]
        public string? BillGroup { get; set; }

        [Column("bill_bank_id")]
        public int BillBankId { get; set; }

        [Column("bill_datefrom")]
        public DateTime? BillDateFrom { get; set; }

        [Column("bill_dateto")]
        public DateTime? BillDateTo { get; set; }

        [Column("bill_pincode")]
        [StringLength(10)]
        public string? BillPincode { get; set; }

        [Column("bill_qrcode")]
        public byte[]? BillQRCode { get; set; }

        [Column("bill_report_id")]
        public int BillReportId { get; set; }

        [Column("bill_cbmqty")]
        public float BillCbmQty { get; set; }

        [Column("bill_remarks_default")]
        [StringLength(300)]
        public string? BillRemarksDefault { get; set; }

        [Column("bill_consignor")]
        [StringLength(100)]
        public string? BillConsignor { get; set; }

        [Column("bill_cust7")]
        [StringLength(100)]
        public string? BillCust7 { get; set; }

        [Column("bill_cust8")]
        [StringLength(100)]
        public string? BillCust8 { get; set; }

        [Column("bill_cust9")]
        [StringLength(100)]
        public string? BillCust9 { get; set; }

        [Column("bill_cust10")]
        [StringLength(100)]
        public string? BillCust10 { get; set; }

        [Column("bill_uuid")]
        [StringLength(45)]
        public string? BillUuid { get; set; }

        [Column("bill_taxableamt2")]
        public float BillTaxableAmt2 { get; set; }

        [Column("bill_gsttype")]
        [StringLength(30)]
        public string? BillGstType { get; set; }

        [Column("bill_jobid")]
        public int BillJobId { get; set; }

        [Column("bill_cdn_reason")]
        [StringLength(200)]
        public string? BillCdnReason { get; set; }

        [Column("bill_locked_by")]
        public int BillLockedBy { get; set; }

        [Column("bill_approved_by")]
        public int BillApprovedBy { get; set; }

        [Column("bill_drcr_acc_id")]
        public int BillDrcrAccId { get; set; }

        [Column("bill_advance")]
        public float BillAdvance { get; set; }

        [Column("bill_netamount")]
        public float BillNetAmount { get; set; }

        [Column("bill_tds_amt")]
        public float BillTdsAmt { get; set; }

        [Column("bill_tds_per")]
        public float BillTdsPer { get; set; }

        [Column("bill_against_billid")]
        [StringLength(50)]
        public string? BillAgainstBillId { get; set; }

        [Column("bill_isrcm")]
        public bool BillIsRcm { get; set; }

        [Column("bill_branch_id")]
        public int BillBranchId { get; set; }

        [Column("bill_hblno")]
        [StringLength(40)]
        public string? BillHblNo { get; set; }

        [Column("bill_ship_party_id")]
        public int BillShipPartyId { get; set; }

        [Column("bill_tcs_per")]
        public double BillTcsPer { get; set; }

        [Column("bill_tcs_amt")]
        public double BillTcsAmt { get; set; }

        // Navigation properties
        [InverseProperty(nameof(BillDetail.Bill))]
        public ICollection<BillDetail>? BillDetails { get; set; }

  
      //  public ICollection<BillRefDetail>? BillRefDetails { get; set; }
    }
}
