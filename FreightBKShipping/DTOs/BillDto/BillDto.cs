using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.DTOs.BillDto
{
    public class BillDto
    {
   
        public int BillId { get; set; }
        public int BillCompanyId { get; set; }
        public string? BillAddedByUserId { get; set; }
        public string? BillUpdatedByUserId { get; set; }
        public int BillPartyId { get; set; }
        public int BillVoucherId { get; set; }
        public int BillYearId { get; set; }
        public string? BillSalesmanId { get; set; }
        public string? BillNo { get; set; }
        public int BillVchNo { get; set; }
        public DateTime BillDate { get; set; }
        public string? BillTime { get; set; }
        public DateTime? BillDueDate { get; set; }
        public string? BillType { get; set; }
        public double BillAmount { get; set; }
        public double BillDisper1 { get; set; }
        public double BillDiscount1 { get; set; }
        public double BillIgst { get; set; }
        public double BillSgst { get; set; }
        public double BillCgst { get; set; }
        public double BillTaxableAmt { get; set; }
        public double BillNonTaxable { get; set; }
        public double BillRoundAmt { get; set; }
        public bool BillIsRoundOff { get; set; }
        public int BillGstIdFreight { get; set; }
        public int BillGstIdCharge { get; set; }
        public double BillGstAmtFreight { get; set; }
        public double BillGstAmtCharge { get; set; }
        public double BillTotalUsd { get; set; }
        public double BillTotal { get; set; }
        public int BillPlaceOfSupply { get; set; }
        public string? BillSupplyType { get; set; }
        public string? BillShipParty { get; set; }
        public string? BillAddress1 { get; set; }
        public string? BillAddress2 { get; set; }
        public string? BillAddress3 { get; set; }
        public string? BillCity { get; set; }
        public string? BillContactNo { get; set; }
        public string? BillGstNo { get; set; }
        public int BillStateId { get; set; }
        public string? BillAgainstBillDate { get; set; }
        public string? BillAgainstBillNo { get; set; }
        public string? BillDrCr { get; set; }
        public bool BillIsCancel { get; set; }
        public bool BillIsFreeze { get; set; }
        public bool BillTaxIncluded { get; set; }
        public string? BillBy { get; set; }
        public string? BillRemarks { get; set; }
        public string? BillAmountInWord { get; set; }
        public string? BillJobNo { get; set; }
        public string? BillJobType { get; set; }
        public int BillPodId { get; set; }
        public int BillPolId { get; set; }
        public int BillVesselId { get; set; }
        public int BillLineId { get; set; }
        public int BillCargoId { get; set; }
        public int BillConsigneeId { get; set; }
        public int BillShipperId { get; set; }
        public string? BillSbNo { get; set; }
        public DateTime? BillSbDate { get; set; }
        public string? BillBlNo { get; set; }
        public DateTime? BillBlDate { get; set; }
        public string? BillShipperInvNo { get; set; }
        public DateTime? BillShipperInvDate { get; set; }
        public double BillGrossWt { get; set; }
        public double BillNetWt { get; set; }
        public double BillQty { get; set; }
        public double BillExchRate { get; set; }
        public string? Bill20Ft { get; set; }
        public string? Bill40Ft { get; set; }
        public string? BillContainerNo { get; set; }
        public string? BillCust1 { get; set; }
        public string? BillCust2 { get; set; }
        public string? BillCust3 { get; set; }
        public string? BillCust4 { get; set; }
        public string? BillCust5 { get; set; }
        public string? BillCust6 { get; set; }
        public string? BillIrnNo { get; set; }
        public string? BillAckNo { get; set; }
        public string? BillAckDate { get; set; }
        public bool BillStatus { get; set; }
        public DateTime BillCreated { get; set; }
        public DateTime? BillUpdated { get; set; }
        public string? BillPrefix { get; set; }
        public string? BillPostfix { get; set; }
        public int BillDefaultCurrencyId { get; set; }
        public string? BillGroup { get; set; }
        public int BillBankId { get; set; }
        public DateTime? BillDateFrom { get; set; }
        public DateTime? BillDateTo { get; set; }
        public string? BillPincode { get; set; }
        public byte[]? BillQrCode { get; set; } = null;
        public int BillReportId { get; set; }
        public float BillCbmQty { get; set; }
        public string? BillRemarksDefault { get; set; }
        public string? BillConsignor { get; set; }
        public string? BillCust7 { get; set; }
        public string? BillCust8 { get; set; }
        public string? BillCust9 { get; set; }
        public string? BillCust10 { get; set; }
        public string? BillUuid { get; set; }
        public float BillTaxableAmt2 { get; set; }
        public string? BillGstType { get; set; }
        public int BillJobId { get; set; }
        public string? BillCdnReason { get; set; }
        public string? BillLockedBy { get; set; } = "";

        // ✅ Add username display field
        public string? BillLockedByUsername { get; set; }

        public int BillApprovedBy { get; set; }
        public int BillDrCrAccId { get; set; }
        public float BillAdvance { get; set; }
        public float BillNetAmount { get; set; }
        public float BillTdsAmt { get; set; }
        public float BillTdsPer { get; set; }
        public string? BillAgainstBillId { get; set; }
        public bool BillIsRcm { get; set; }
        public int BillBranchId { get; set; }
        public string? BillHblNo { get; set; }
        public int BillShipPartyId { get; set; }
        public double BillTcsPer { get; set; }
        public double BillTcsAmt { get; set; }
        //NOT MAPPED IN DB    
        public string? partyname { get; set; }
        public string? posname { get; set; }
        public string? Vouchname { get; set; }
        // Nested collections
        public List<BillDetailDto> BillDetails { get; set; } = new();
        //public List<BillRefDetailDto> BillRefDetails { get; set; } = new();

        

    }
}
