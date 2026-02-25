using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("lrs")]
    public class Lr
    {
        [Key]
        [Column("lr_Id")]
        public int LrId { get; set; }
        [ValidateNever]
        [Column("lr_company_id")]
        public int LrCompanyId { get; set; }
        [ValidateNever]
        [Column("lr_addedby_user_id")]
        public string LrAddedByUserId { get; set; }
        [ValidateNever]
        [Column("lr_updatedby_user_id")]
        public string? LrUpdatedByUserId { get; set; }

        [Column("lr_voucher_id")]
        public int LrVoucherId { get; set; }

        [Column("lr_party_account_id")]
        public int LrPartyAccountId { get; set; }

        [Column("lr_consignee_notify_id")]
        public int? LrConsigneeNotifyId { get; set; }

        [Column("lr_consignor_notify_id")]
        public int? LrConsignorNotifyId { get; set; }

        [Column("lr_product_id")]
        public int? LrProductId { get; set; }

        [Column("lr_vehicle_id")]
        public int LrVehicleId { get; set; }

        [Column("lr_supplier_account_id")]
        public int LrSupplierAccountId { get; set; }

        [Column("lr_driver_id")]
        public int? LrDriverId { get; set; }

        [Column("lr_no")]
        public int? LrNo { get; set; }

        [Column("lr_nostr")]
        public string? LrNoStr { get; set; }

        [Column("lr_date")]
        public DateTime? LrDate { get; set; }

        [Column("lr_time")]
        public string? LrTime { get; set; }

        [Column("lr_tripno")]
        public int? LrTripNo { get; set; }

        [Column("lr_from_location_id")]
        public int LrFromLocationId { get; set; }

        [Column("lr_to_location_id")]
        public int LrToLocationId { get; set; }

        [Column("lr_back_location_id")]
        public int? LrBackLocationId { get; set; }

        [Column("lr_container1")]
        public string? LrContainer1 { get; set; }

        [Column("lr_container2")]
        public string? LrContainer2 { get; set; }

        [Column("lr_grosswt")]
        public double? LrGrossWt { get; set; }

        [Column("lr_tarewt")]
        public double? LrTareWt { get; set; }

        [Column("lr_loadwt")]
        public double LrLoadWt { get; set; } = 0;

        [Column("lr_chargeqty")]
        public double LrChargeQty { get; set; } = 0;

        [Column("lr_unloadwt")]
        public double LrUnloadWt { get; set; } = 0;

        [Column("lr_shortwt")]
        public double LrShortWt { get; set; } = 0;

        [Column("lr_shortallow_bill")]
        public double LrShortAllowBill { get; set; } = 0;

        [Column("lr_short_per_bill")]
        public double LrShortPerBill { get; set; } = 0;

        [Column("lr_short_per_truck")]
        public float LrShortPerTruck { get; set; } = 0;

        [Column("lr_shortallowtype")]
        public string? LrShortAllowType { get; set; } // % / Fixed

        [Column("lr_shortnet_bill")]
        public float LrShortNetBill { get; set; } = 0;

        [Column("lr_paymenttype")]
        public string? LrPaymentType { get; set; } // To Pay / Paid / To be Billed

        [Column("lr_ratebill")]
        public double LrRateBill { get; set; } = 0;

        [Column("lr_grossfreightbill")]
        public double LrGrossFreightBill { get; set; } = 0;

        [Column("lr_tripchargebill")]
        public double LrTripChargeBill { get; set; } = 0;

        [Column("lr_advancebill")]
        public double LrAdvanceBill { get; set; } = 0;

        [Column("lr_netfreightbill")]
        public double LrNetFreightBill { get; set; } = 0;

        [Column("lr_refby")]
        public string? LrRefBy { get; set; }

        [Column("lr_startkm")]
        public string LrStartKm { get; set; } = "0";

        [Column("lr_endkm")]
        public string LrEndKm { get; set; } = "0";

        [Column("lr_custom1")]
        public string? LrCustom1 { get; set; }

        [Column("lr_custom2")]
        public string? LrCustom2 { get; set; }

        [Column("lr_custom3")]
        public string? LrCustom3 { get; set; }

        [Column("lr_custom4")]
        public string? LrCustom4 { get; set; }

        [Column("lr_remarks")]
        public string? LrRemarks { get; set; }

        // 0->Inactive, 1->Active, 2->Delete
        [Column("lr_status")]
        public int LrStatus { get; set; } = 1;
        [ValidateNever]
        [Column("lr_created")]
        public DateTime LrCreated { get; set; } = DateTime.UtcNow;
        [ValidateNever]
        [Column("lr_updated")]
        public DateTime? LrUpdated { get; set; } = DateTime.UtcNow;



        [Column("lr_nt_size")]
        public string? LrNtSize { get; set; }

        [Column("lr_nt_seal_no")]
        public string? LrNtSealNo { get; set; }

        [Column("lr_nt_date")]
        public DateTime? LrNtDate { get; set; }

        [Column("lr_nt_rfidseal")]
        public string? LrNtRfidSeal { get; set; }

        [Column("lr_nt_qty")]
        public decimal? LrNtQty { get; set; }

        [Column("lr_nt_invNo")]
        public string? LrNtInvNo { get; set; }

        [Column("lr_nt_pickupdt")]
        public DateTime? LrNtPickupDt { get; set; }

        [Column("lr_nt_pickuploc")]
        public string? LrNtPickupLoc { get; set; }

        [Column("lr_nt_shipingbillNo")]
        public string? LrNtShipingBillNo { get; set; }

        [Column("lr_nt_unit")]
        public string? LrNtUnit { get; set; }

        [Column("lr_nt_netwt")]
        public decimal? LrNtNetWt { get; set; }

        [Column("lr_nt_cbm")]
        public decimal? LrNtCbm { get; set; }

        [Column("lr_nt_vehicleNo")]
        public string? LrNtVehicleNo { get; set; }

        [Column("lr_nt_transporter")]
        public string? LrNtTransporter { get; set; }

        [Column("lr_nt_gateoutdt")]
        public DateTime? LrNtGateOutDt { get; set; }

        [Column("lr_nt_gateindt")]
        public DateTime? LrNtGateInDt { get; set; }

        [Column("lr_nt_dischargedt")]
        public DateTime? LrNtDischargeDt { get; set; }

        //=======
        // 🔹 Consignor Notify Address
        [Column("lr_consignor_notify_address")]
        [StringLength(500)]
        public string? LrConsignorNotifyAddress { get; set; }

        // 🔹 Consignee Notify Address
        [Column("lr_consignee_notify_address")]
       
        public string? LrConsigneeNotifyAddress { get; set; }

        // 🔹 Consignor Notify GST
        [Column("lr_consignor_notify_gst")]
       
        public string? LrConsignorNotifyGst { get; set; }

        // 🔹 Consignor Notify State
        [Column("lr_consignor_notify_state")]
     
        public string? LrConsignorNotifyState { get; set; }

        // 🔹 Consignee Notify GST
        [Column("lr_consignee_notify_gst")]
     
        public string? LrConsigneeNotifyGst { get; set; }

        // 🔹 Consignee Notify State
        [Column("lr_consignee_notify_state")]
    
        public string? LrConsigneeNotifyState { get; set; }

        // ================= Billing / Truck =================
        [Column("lr_billtypebill")]
        public string? LrBillTypeBill { get; set; }

        [Column("lr_billtypetruck")]
        public int? LrBillTypeTruck { get; set; }

        [Column("lr_billratetruck")]
        public double LrBillRateTruck { get; set; }

        [Column("lr_grossfreighttruck")]
        public double LrGrossFreightTruck { get; set; }

        [Column("lr_tripchargetruck")]
        public double LrTripChargeTruck { get; set; }

        [Column("lr_tripadvance")]
        public double LrTripAdvance { get; set; }

        [Column("lr_cashbank")]
        public double LrCashBank { get; set; }

        [Column("lr_netfreighttruck")]
        public double LrNetFreightTruck { get; set; }

        [Column("lr_shortratebill")]
        public double LrShortRateBill { get; set; }

        [Column("lr_shortratetruck")]
        public double LrShortRateTruck { get; set; }

        [Column("lr_shortamtbill")]
        public double LrShortAmtBill { get; set; }

        [Column("lr_shortamttruck")]
        public double LrShortAmtTruck { get; set; }


        // ================= Invoice / Eway =================
        [Column("lr_invoiceno")]
        public string? LrInvoiceNo { get; set; }

        [Column("lr_invoicevalue")]
        public double LrInvoiceValue { get; set; }

        [Column("lr_invoicedate")]
        public string? LrInvoiceDate { get; set; }

        [Column("lr_dono")]
        public string? LrDoNo { get; set; }

        [Column("lr_dodate")]
        public string? LrDoDate { get; set; }

        [Column("lr_ewaybillno")]
        public string? LrEwayBillNo { get; set; }

        [Column("lr_sbno")]
        public string? LrSbNo { get; set; }

        [Column("lr_ewaybill_expdt")]
        public DateTime? LrEwayBillExpDt { get; set; }


        // ================= Report / POD =================
        [Column("lr_report_date")]
        public string? LrReportDate { get; set; }

        [Column("lr_report_time")]
        public string? LrReportTime { get; set; }

        [Column("lr_unload_date")]
        public string? LrUnloadDate { get; set; }

        [Column("lr_load_date")]
        public string? LrloadDate { get; set; }


        [Column("lr_unload_time")]
        public string? LrUnloadTime { get; set; }

        [Column("lr_pod_date")]
        public string? LrPodDate { get; set; }

        [Column("lr_return_date")]
        public string? LrReturnDate { get; set; }

        [Column("lr_pod_submitted")]
        public string? LrPodSubmitted { get; set; }


        // ================= Detention =================
        [Column("lr_detention_ratebill")]
        public double LrDetentionRateBill { get; set; }

        [Column("lr_detention_ratetruck")]
        public double LrDetentionRateTruck { get; set; }

        [Column("lr_detention_daybill")]
        public double LrDetentionDayBill { get; set; }

        [Column("lr_detention_daytruck")]
        public double LrDetentionDayTruck { get; set; }

        [Column("lr_detention_amtbill")]
        public double LrDetentionAmtBill { get; set; }

        [Column("lr_detention_amttruck")]
        public double LrDetentionAmtTruck { get; set; }


        // ================= GST / TDS / Commission =================
        [Column("lr_gst_percentage")]
        public float LrGstPercentage { get; set; }

        [Column("lr_gst_amount")]
        public float LrGstAmount { get; set; }

        [Column("lr_gst_slab_id")]
        public int? LrGstSlabId { get; set; }

        [Column("lr_GstPayableBy")]
        public string? LrGstPayableBy { get; set; }

        [Column("lr_tds_per_bill")]
        public float LrTdsPerBill { get; set; }

        [Column("lr_tds_amt_bill")]
        public float LrTdsAmtBill { get; set; }

        [Column("lr_comm_per_bill")]
        public float LrCommPerBill { get; set; }

        [Column("lr_comm_amt_bill")]
        public float LrCommAmtBill { get; set; }

        [Column("lr_tds_per_truck")]
        public float LrTdsPerTruck { get; set; }

        [Column("lr_tds_amt_truck")]
        public float LrTdsAmtTruck { get; set; }

        [Column("lr_comm_per_truck")]
        public float LrCommPerTruck { get; set; }

        [Column("lr_comm_amt_truck")]
        public float LrCommAmtTruck { get; set; }


        // ================= Agent / Job =================
        [Column("lr_agent_id")]
        public int LrAgentId { get; set; }

        [Column("lr_agent_rate")]
        public float LrAgentRate { get; set; }

        [Column("lr_agent_amount")]
        public float LrAgentAmount { get; set; }

        [Column("lr_agent_paytype")]
        public int LrAgentPayType { get; set; }

        [Column("lr_job_id")]
        public int LrJobId { get; set; }

        [Column("lr_job_no")]
        public string? LrJobNo { get; set; }

        [Column("lr_locked_by")]
        public int LrLockedBy { get; set; }


        // ================= Misc =================
        [Column("lr_totkm")]
        public string? LrTotKm { get; set; }

        [Column("lr_totalbags")]
        public decimal LrTotalBags { get; set; }

        [Column("lr_triptype")]
        public string? LrTripType { get; set; }

        [Column("lr_groupno")]
        public string? LrGroupNo { get; set; }

        [Column("lr_allow_amt")]
        public double LrAllowAmt { get; set; }

        [Column("lr_allow_diesel_qty")]
        public double LrAllowDieselQty { get; set; }

        [Column("lr_shortallow_truck")]
        public float LrShortAllowTruck { get; set; }

        [Column("lr_shortnet_truck")]
        public float LrShortNetTruck { get; set; }

        [Column("lr_refName")]
        public string? LrRefName { get; set; }

        [Column("lr_topay_amt")]
        public float LrTopayAmt { get; set; }

        [Column("lr_topay_mode")]
        public string? LrTopayMode { get; set; }

        [Column("lr_printformat_id")]
        public int LrPrintFormatId { get; set; }

        [Column("lr_urea_opening")]
        public float LrUreaOpening { get; set; }

        [Column("lr_diesel_opening")]
        public float LrDieselOpening { get; set; }

        [Column("lr_urea_qty")]
        public float LrUreaQty { get; set; }

        [Column("lr_other_chargebill")]
        public float LrOtherChargeBill { get; set; }

        [Column("lr_other_chargebill2")]
        public double LrOtherChargeBill2 { get; set; }

        [Column("lr_diesel_closing")]
        public float LrDieselClosing { get; set; }

        [Column("lr_urea_closing")]
        public float LrUreaClosing { get; set; }

        [Column("lr_hamali")]
        public float LrHamali { get; set; }

        [Column("lr_driver_mobile")]
        public string? LrDriverMobile { get; set; }

        [Column("lr_journal_amt")]
        public float LrJournalAmt { get; set; }

        [Column("lr_type")]
        public string? LrType { get; set; }

        [Column("lr_delivery_location")]
        public string? LrDeliveryLocation { get; set; }

        [Column("lr_custom5")]
        public string? LrCustom5 { get; set; }

        [Column("lr_custom6")]
        public string? LrCustom6 { get; set; }

        [Column("lr_custom7")]
        public string? LrCustom7 { get; set; }

        [Column("lr_custom8")]
        public string? LrCustom8 { get; set; }

        [Column("lr_truckwt")]
        public float LrTruckWt { get; set; }

        [Column("lr_year_id")]
        public int LrYearId { get; set; }

        [Column("lr_branch_id")]
        public int LrBranchId { get; set; }

        [Column("lr_paymode_truck")]
        public string? LrPaymodeTruck { get; set; }

        [Column("lr_bhatta_extra")]
        public double LrBhattaExtra { get; set; }

        [Column("lr_driver_remarks")]
        public double LrDriverRemarks { get; set; }

        [Column("lr_toll_exp")]
        public float LrTollExp { get; set; }

        [Column("lr_diesel_extra")]
        public int LrDieselExtra { get; set; }

        [Column("lr_pickup_location")]
        public string? LrPickupLocation { get; set; }

        [Column("lr_pickup_date")]
        public DateTime? LrPickupDate { get; set; }

        [Column("lr_driver_bhata_remarks")]
        public string? LrDriverBhataRemarks { get; set; }

        [Column("lr_lu_party")]
        public float LrLuParty { get; set; }

        [Column("lr_lu_supplier")]
        public float LrLuSupplier { get; set; }

        [Column("lr_lu_rate")]
        public float LrLuRate { get; set; }

        [Column("lr_cha_id")]
        public int LrChaId { get; set; }

        [Column("lr_vessel_id")]
        public int LrVesselId { get; set; }

        [Column("lr_gatepassno")]
        public string? LrGatePassNo { get; set; }

        [Column("lr_party_bill_id")]
        public int LrPartyBillId { get; set; }

        [Column("lr_supplier_bill_id")]
        public int LrSupplierBillId { get; set; }

        [Column("lr_allowance_pay_id")]
        public int LrAllowancePayId { get; set; }

        [Column("lr_advance_pay_id")]
        public int LrAdvancePayId { get; set; }

        [Column("lr_advance_rec_id")]
        public int LrAdvanceRecId { get; set; }



    }
    public class LrListVM
    {
        public int LrId { get; set; }

        // Basic
        public string? LrNoStr { get; set; }
        public DateTime? LrDate { get; set; }
        public string? LrTripNo { get; set; }

        // Party Info (Names only)
        public string? PartyName { get; set; }
        public string? SupplierName { get; set; }
        public string? VehicleNo { get; set; }
        public string? DriverName { get; set; }

        // Location (Names only)
        public string? FromLocationName { get; set; }
        public string? ToLocationName { get; set; }

        // Weight
        public double LrLoadWt { get; set; }
        public double LrUnloadWt { get; set; }
        public double LrShortWt { get; set; }

        // Billing
        public string? LrBillTypeBill { get; set; }
        public double LrRateBill { get; set; }
        public double LrGrossFreightBill { get; set; }
        public double LrNetFreightBill { get; set; }

        // Truck
        public double LrBillRateTruck { get; set; }
        public double LrNetFreightTruck { get; set; }

        // GST
        public double LrGstPercentage { get; set; }
        public double LrGstAmount { get; set; }

        // Status
        public int LrStatus { get; set; }
    }
    public class LrEntryDto
    {
        public Lr Main { get; set; } = new();
        public List<LRDetail> Details { get; set; } = new();
        public List<LRJournal> Journals { get; set; } = new();
    }
}
