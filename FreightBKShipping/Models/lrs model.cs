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

        [Column("lr_company_id")]
        public int LrCompanyId { get; set; }

        [Column("lr_addedby_user_id")]
        public int LrAddedByUserId { get; set; }

        [Column("lr_updatedby_user_id")]
        public int? LrUpdatedByUserId { get; set; }

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

        [Column("lr_created")]
        public DateTime LrCreated { get; set; } = DateTime.UtcNow;

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

    }
}
