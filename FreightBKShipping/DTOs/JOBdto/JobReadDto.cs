namespace FreightBKShipping.DTOs
{
    public class JobReadDto
    {
        public int JobId { get; set; }
        public string? JobCompanyId { get; set; }
        public string? JobAddedByUserId { get; set; }
        public string? JobUpdatedByUserId { get; set; }
        public int? JobPartyId { get; set; }
        public string? JobYearId { get; set; }
        public DateTime? JobDate { get; set; }
        public string? JobNo { get; set; }
        public string? JobType { get; set; }
        public int? JobPodId { get; set; }
        public int? JobPolId { get; set; }
        public int? JobVesselId { get; set; }
        public int? JobLineId { get; set; }
        public int? JobCargoId { get; set; }
        public int? JobConsigneeId { get; set; }
        public int? JobShipperId { get; set; }
        public int? JobSalesmanId { get; set; }
        public string? JobSbNo { get; set; }
        public DateTime? JobSbDate { get; set; }
        public string? JobBlNo { get; set; }
        public DateTime? JobBlDate { get; set; }
        public string? JobShipperInvNo { get; set; }
        public DateTime? JobShipperInvDate { get; set; }
        public double? JobGrossWt { get; set; }
        public double? JobNetWt { get; set; }
        public double? JobQty { get; set; }
        public double? JobExchRate { get; set; }
        public string? Job20Ft { get; set; }
        public string? Job40Ft { get; set; }
        public string? JobContainer20Ft { get; set; }
        public string? JobContainer40Ft { get; set; }
        public int? JobDefCurrId { get; set; }
        public string? JobRemarks { get; set; }
        public byte? JobStatus { get; set; }
        public DateTime JobCreated { get; set; }
        public DateTime JobUpdated { get; set; }
        public int? JobVchNo { get; set; }
        public string? JobPrefix { get; set; }
        public string? JobSufix { get; set; }
        public string? JobState { get; set; }
        public int? JobTypeId { get; set; }

        public string? JobCust1 { get; set; }
        public string? JobCust2 { get; set; }
        public string? JobCust3 { get; set; }
        public string? JobCust4 { get; set; }
        public string? JobCust5 { get; set; }
        public string? JobCust6 { get; set; }
        public string? JobCust7 { get; set; }
        public string? JobCust8 { get; set; }
        public string? JobCust9 { get; set; }

        public int? JobChaId { get; set; }
        public string? JobBeNo { get; set; }
        public DateTime? JobBeDate { get; set; }
        public int? JobSupplierId { get; set; }
        public float? JobDoPer { get; set; }
        public DateTime? JobDoDate { get; set; }
        public string? JobDoNo { get; set; }
        public DateTime? JobOutOfChargeDate { get; set; }
        public DateTime? JobObgDate { get; set; }
        public DateTime? JobOblDate { get; set; }
        public DateTime? JobFormAI { get; set; }
        public DateTime? JobPaymentReceivedDate { get; set; }
        public string? JobGoodsDesc { get; set; }

        public int? JobHighSeas1 { get; set; }
        public int? JobHighSeas2 { get; set; }
        public string? JobCountryOrigin { get; set; }
        public float? JobCbm { get; set; }
        public string? JobQtyUnit { get; set; }
        public string? JobGrossUnit { get; set; }
        public string? JobNetUnit { get; set; }
        public string? JobContainerType { get; set; }
        public string? JobBlType { get; set; }
        public string? JobVoy { get; set; }
        public string? JobCfs { get; set; }
        public string? JobEmptyYard { get; set; }
        public string? JobVolume { get; set; }

        public int? JobLockedBy { get; set; }
        public int? JobApprovedBy { get; set; }
        public string? JobForwarder { get; set; }
        public string? JobForwarderAddress { get; set; }
        public string? JobCountryDischarge { get; set; }
        public string? JobBookingNo { get; set; }
        public string? JobHsnCode { get; set; }
        public string? JobHblNo { get; set; }
        public DateTime? JobHblDate { get; set; }
        public DateTime? JobCompleteDate { get; set; }
        public string? JobMarks { get; set; }

        public string? JobPrecarriedBy { get; set; }
        public string? JobPlaceOfReceipt { get; set; }
        public string? JobPlaceOfDelivery { get; set; }
        public string? JobOnCarries { get; set; }
        public string? JobCertiOrigin { get; set; }
        public string? JobMeasurement { get; set; }
        public string? JobMtdNo { get; set; }
        public string? JobBrand { get; set; }

        public string? JobIgmNo { get; set; }
        public DateTime? JobIgmDate { get; set; }
        public string? JobDoType { get; set; }
        public string? JobIcd { get; set; }
        public string? JobTerminal { get; set; }
        public int? JobFreeDays { get; set; }
        public DateTime? JobEta { get; set; }
        public DateTime? JobEtd { get; set; }
        public string? JobSealNo { get; set; }

        public string? Surveyor { get; set; }
        public string? SurveyorAddress { get; set; }
        public int? JobBranchId { get; set; }
        public DateTime? JobDoValid { get; set; }
        public int? JobDescSplitLine { get; set; }

        public string? JobGoodsDesc1 { get; set; }
        public string? JobGoodsDesc2 { get; set; }
        public string? JobTranshipment { get; set; }
        public string? JobTransTime { get; set; }
        public string? JobPtaFta { get; set; }
        public string? JobPhytoStatus { get; set; }
        public string? JobFumigationStatus { get; set; }
        public string? JobOtherCert { get; set; }
        public string? JobGoodsStuffed { get; set; }
        public string? JobFreightBy { get; set; }
        public string? JobFreightRemarks { get; set; }

        public string? JobIssuePlace { get; set; }
        public string? JobNoOfBl { get; set; }
        public string? JobShipperAddress { get; set; }
        public string? JobConsigneeAddress { get; set; }
        public string? JobNotifyAddress { get; set; }
        public string? JobAgentAddress { get; set; }
        public string? JobSobDt { get; set; }
        public string? JobCrono { get; set; }
        public string? JobBookingParty { get; set; }
        public string? JobAcceptionPlace { get; set; }
        public string? JobAcceptionDt { get; set; }
        public int? JobBlSeriesId { get; set; }
        public string? JobAgent { get; set; }
        public string? JobPartyAddress { get; set; }
        public string? JobHighseas1Address { get; set; }

        // ✅ Joined / UI fields
        public string? VesselName { get; set; }
        public string? PolName { get; set; }
        public string? PodName { get; set; }
        public string? BranchName { get; set; }
        public string? Partyname { get; set; }

    }
}
