using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("jobs")]
    public class Job
    {
        [Key]
        [Column("job_id")]
        public int JobId { get; set; }

        [Column("job_company_id")]
        public string? JobCompanyId { get; set; }

        [Column("job_addedby_user_id")]
        public string? JobAddedByUserId { get; set; }

        [Column("job_updatedby_user_id")]
        public string? JobUpdatedByUserId { get; set; }

        [Column("job_partyid")]
        public int? JobPartyId { get; set; }

        [Column("job_year_id")]
        public string? JobYearId { get; set; }

        [Column("job_date")]
        public DateTime? JobDate { get; set; }

        [Column("job_no")]
        public string? JobNo { get; set; }

        [Column("job_type")]
        public string? JobType { get; set; }

        [Column("job_pod_id")]
        public int? JobPodId { get; set; }

        [Column("job_pol_id")]
        public int? JobPolId { get; set; }

        [Column("job_vessel_id")]
        public int? JobVesselId { get; set; }

        [Column("job_line_id")]
        public int? JobLineId { get; set; }

        [Column("job_cargo_id")]
        public int? JobCargoId { get; set; }

        [Column("job_consignee_id")]
        public int? JobConsigneeId { get; set; }

        [Column("job_shipper_id")]
        public int? JobShipperId { get; set; }

        [Column("job_salesman_id")]
        public int? JobSalesmanId { get; set; }

        [Column("job_sbno")]
        public string? JobSbNo { get; set; }

        [Column("job_sbdate")]
        public DateTime? JobSbDate { get; set; }

        [Column("job_blno")]
        public string? JobBlNo { get; set; }

        [Column("job_bldate")]
        public DateTime? JobBlDate { get; set; }

        [Column("job_shipper_inv_no")]
        public string? JobShipperInvNo { get; set; }

        [Column("job_shipper_inv_date")]
        public DateTime? JobShipperInvDate { get; set; }

        [Column("job_grosswt")]
        public double? JobGrossWt { get; set; }

        [Column("job_netwt")]
        public double? JobNetWt { get; set; }

        [Column("job_qty")]
        public double? JobQty { get; set; }

        [Column("job_exchrate")]
        public double? JobExchRate { get; set; }

        [Column("job_20ft")]
        public string? Job20Ft { get; set; }

        [Column("job_40ft")]
        public string? Job40Ft { get; set; }

        [Column("job_container_20ft")]
        public string? JobContainer20Ft { get; set; }

        [Column("job_container_40ft")]
        public string? JobContainer40Ft { get; set; }

        [Column("job_def_curr_id")]
        public int? JobDefCurrId { get; set; }

        [Column("job_remarks")]
        public string? JobRemarks { get; set; }

        [Column("job_status")]
        public byte? JobStatus { get; set; }

        [Column("job_created")]
        public DateTime JobCreated { get; set; } = DateTime.UtcNow;

        [Column("job_updated")]
        public DateTime JobUpdated { get; set; } = DateTime.UtcNow;

        [Column("job_vch_no")]
        public int? JobVchNo { get; set; }

        [Column("job_prefix")]
        public string? JobPrefix { get; set; }

        [Column("job_sufix")]
        public string? JobSufix { get; set; }

        [Column("job_state")]
        public string? JobState { get; set; }

        [Column("job_type_id")]
        public int? JobTypeId { get; set; }

        [Column("job_cust1")]
        public string? JobCust1 { get; set; }

        [Column("job_cust2")]
        public string? JobCust2 { get; set; }

        [Column("job_cust3")]
        public string? JobCust3 { get; set; }

        [Column("job_cust4")]
        public string? JobCust4 { get; set; }

        [Column("job_cust5")]
        public string? JobCust5 { get; set; }

        [Column("job_cust6")]
        public string? JobCust6 { get; set; }

        [Column("job_cust7")]
        public string? JobCust7 { get; set; }

        [Column("job_cust8")]
        public string? JobCust8 { get; set; }

        [Column("job_cust9")]
        public string? JobCust9 { get; set; }

        [Column("job_cha_id")]
        public int? JobChaId { get; set; }

        [Column("job_be_no")]
        public string? JobBeNo { get; set; }

        [Column("job_be_date")]
        public DateTime? JobBeDate { get; set; }

        [Column("job_supplier_id")]
        public int? JobSupplierId { get; set; }

        [Column("job_do_per")]
        public float? JobDoPer { get; set; }

        [Column("job_do_date")]
        public DateTime? JobDoDate { get; set; }

        [Column("job_dono")]
        public string? JobDoNo { get; set; }

        [Column("job_outofcharg_date")]
        public DateTime? JobOutOfChargeDate { get; set; }

        [Column("job_obg_date")]
        public DateTime? JobObgDate { get; set; }

        [Column("job_obl_date")]
        public DateTime? JobOblDate { get; set; }

        [Column("job_formAI")]
        public DateTime? JobFormAI { get; set; }

        [Column("job_payment_rec_date")]
        public DateTime? JobPaymentReceivedDate { get; set; }

        [Column("job_goods_desc")]
        public string? JobGoodsDesc { get; set; }

        [Column("job_highseas1")]
        public int? JobHighSeas1 { get; set; }

        [Column("job_highseas2")]
        public int? JobHighSeas2 { get; set; }

        [Column("job_country_origin")]
        public string? JobCountryOrigin { get; set; }

        [Column("job_cbm")]
        public float? JobCbm { get; set; }

        [Column("job_qty_unit")]
        public string? JobQtyUnit { get; set; }

        [Column("job_gross_unit")]
        public string? JobGrossUnit { get; set; }

        [Column("job_net_unit")]
        public string? JobNetUnit { get; set; }

        [Column("job_container_type")]
        public string? JobContainerType { get; set; }

        [Column("job_bl_type")]
        public string? JobBlType { get; set; }

        [Column("job_voy")]
        public string? JobVoy { get; set; }

        [Column("job_cfs")]
        public string? JobCfs { get; set; }

        [Column("job_empty_yard")]
        public string? JobEmptyYard { get; set; }

        [Column("job_volume")]
        public string? JobVolume { get; set; }

        [Column("job_locked_by")]
        public int? JobLockedBy { get; set; }

        [Column("job_approved_by")]
        public int? JobApprovedBy { get; set; }

        [Column("job_forwarder")]
        public string? JobForwarder { get; set; }

        [Column("job_forwarder_address")]
        public string? JobForwarderAddress { get; set; }

        [Column("job_country_discharge")]
        public string? JobCountryDischarge { get; set; }

        [Column("job_booking_no")]
        public string? JobBookingNo { get; set; }

        [Column("job_hsncode")]
        public string? JobHsnCode { get; set; }

        [Column("job_hbl_no")]
        public string? JobHblNo { get; set; }

        [Column("job_hbl_date")]
        public DateTime? JobHblDate { get; set; }

        [Column("job_complete_date")]
        public DateTime? JobCompleteDate { get; set; }

        [Column("job_marks")]
        public string? JobMarks { get; set; }

        [Column("job_precarried_by")]
        public string? JobPrecarriedBy { get; set; }

        [Column("job_place_of_receipt")]
        public string? JobPlaceOfReceipt { get; set; }

        [Column("job_place_of_delivery")]
        public string? JobPlaceOfDelivery { get; set; }

        [Column("job_oncarries")]
        public string? JobOnCarries { get; set; }

        [Column("job_certi_origin")]
        public string? JobCertiOrigin { get; set; }

        [Column("job_measurement")]
        public string? JobMeasurement { get; set; }

        [Column("job_mtdno")]
        public string? JobMtdNo { get; set; }

        [Column("job_brand")]
        public string? JobBrand { get; set; }

        [Column("job_igm_no")]
        public string? JobIgmNo { get; set; }

        [Column("job_igm_date")]
        public DateTime? JobIgmDate { get; set; }

        [Column("job_do_type")]
        public string? JobDoType { get; set; }

        [Column("job_icd")]
        public string? JobIcd { get; set; }

        [Column("job_terminal")]
        public string? JobTerminal { get; set; }

        [Column("job_free_days")]
        public int? JobFreeDays { get; set; }

        [Column("job_eta")]
        public DateTime? JobEta { get; set; }

        [Column("job_etd")]
        public DateTime? JobEtd { get; set; }

        [Column("job_sealno")]
        public string? JobSealNo { get; set; }

        [Column("surveyor")]
        public string? Surveyor { get; set; }

        [Column("surveyor_address")]
        public string? SurveyorAddress { get; set; }

        [Column("job_branch_id")]
        public int? JobBranchId { get; set; }

        [Column("job_do_valid")]
        public DateTime? JobDoValid { get; set; }

        [Column("job_desc_splitline")]
        public int? JobDescSplitLine { get; set; }

        [Column("job_goods_desc1")]
        public string? JobGoodsDesc1 { get; set; }

        [Column("job_goods_desc2")]
        public string? JobGoodsDesc2 { get; set; }

        [Column("job_transhipment")]
        public string? JobTranshipment { get; set; }

        [Column("job_trans_time")]
        public string? JobTransTime { get; set; }

        [Column("job_pta_fta")]
        public string? JobPtaFta { get; set; }

        [Column("job_phyto_status")]
        public string? JobPhytoStatus { get; set; }

        [Column("job_fumigation_status")]
        public string? JobFumigationStatus { get; set; }

        [Column("job_other_cert")]
        public string? JobOtherCert { get; set; }

        [Column("job_goods_stuffed")]
        public string? JobGoodsStuffed { get; set; }

        [Column("job_freight_by")]
        public string? JobFreightBy { get; set; }

        [Column("job_freight_remarks")]
        public string? JobFreightRemarks { get; set; }

        [Column("job_issue_place")]
        public string? JobIssuePlace { get; set; }

        [Column("job_noof_bl")]
        public string? JobNoOfBl { get; set; }

        [Column("job_shipper_address")]
        public string? JobShipperAddress { get; set; }

        [Column("job_consignee_address")]
        public string? JobConsigneeAddress { get; set; }

        [Column("job_notify_address")]
        public string? JobNotifyAddress { get; set; }

        [Column("job_agent_address")]
        public string? JobAgentAddress { get; set; }

        [Column("job_sob_dt")]
        public string? JobSobDt { get; set; }

        [Column("job_crono")]
        public string? JobCrono { get; set; }

        [Column("job_booking_party")]
        public string? JobBookingParty { get; set; }

        [Column("job_acception_place")]
        public string? JobAcceptionPlace { get; set; }

        [Column("job_acception_dt")]
        public string? JobAcceptionDt { get; set; }

        [Column("job_bl_series_id")]
        public int? JobBlSeriesId { get; set; }

        [Column("job_agent")]
        [StringLength(300)]
        public string? JobAgent { get; set; }

        [Column("job_party_address")]
        [StringLength(150)]
        public string? JobPartyAddress { get; set; }

        [Column("job_highseas1_address")]
        [StringLength(200)]
        public string? JobHighseas1Address { get; set; }

        //unMapped field in db
        [NotMapped]
        public string? VesselName { get; set; }

        [NotMapped]
        public string? PolName { get; set; }

        [NotMapped]
        public string? PodName { get; set; }

        [NotMapped]
        public string? BranchName { get; set; }

    }
}
