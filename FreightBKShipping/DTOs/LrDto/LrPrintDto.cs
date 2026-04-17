namespace FreightBKShipping.DTOs.LrPrintDto
{
    public class LrPrintDto
    {
        public int LrId { get; set; }
        public string? LrNo { get; set; }
        public DateTime? LrDate { get; set; }
        public string? LrTime { get; set; }
        public int? LrTripNo { get; set; }

        public int LrCompanyId { get; set; }
        public int LrVoucherId { get; set; }
        public string LrPartyAccount { get; set; }
        public string LrSupplierAccount { get; set; }
        public string LrVehicle { get; set; }

        public string LrFromLocation { get; set; }
        public string LrToLocation { get; set; }
        public string? LrBackLocation { get; set; }

        public string? LrContainer1 { get; set; }
        public string? LrContainer2 { get; set; }

        public double? LrGrossWt { get; set; }
        public double? LrTareWt { get; set; }
        public double LrLoadWt { get; set; }
        public double LrUnloadWt { get; set; }
        public double LrShortWt { get; set; }

        public double LrRateBill { get; set; }
        public double LrGrossFreightBill { get; set; }
        public double LrNetFreightBill { get; set; }

        public double LrBillRateTruck { get; set; }
        public double LrNetFreightTruck { get; set; }

        public float LrGstPercentage { get; set; }
        public float LrGstAmount { get; set; }

        public string? LrPaymentType { get; set; }
        public string? LrRemarks { get; set; }

        public double LrAdvanceTotal { get; set; }
        public double LrDieselTotal { get; set; }
        public double LrChargesTotal { get; set; }
        public double LrExpenseTotal { get; set; }
        public double LrAdvRecTotal { get; set; }

        public double LrNetFreightCalc { get; set; }

        public DateTime LrCreated { get; set; }
        public DateTime? LrUpdated { get; set; }


        public decimal LrTotalBags { get; set; }

        public string? LrEwayBillNo { get; set; }
        public DateTime? LrEwayBillExpDt { get; set; }
        public string? LrInvoiceNo { get; set; }
        public DateTime? LrInvoiceDate { get; set; }

        public string? Consignee { get; set; }
        public string? ConsigneeAdd { get; set; }
        public string? Consigneegstin { get; set; }
        public string? Consigneestate { get; set; }
        public string? Consignor { get; set; }
        public string? ConsignorAdd { get; set; }
        public string? Consignorgstin { get; set; }
        public string? Consignorstate { get; set; }

        public string? LrCustom1 { get; set; }
        public string? LrCustom2 { get; set; }
        public string? LrCustom3 { get; set; }
        public string? LrCustom4 { get; set; }
        public string? LrCustom5 { get; set; }
        public string? LrCustom6 { get; set; }
        public string? LrCustom7 { get; set; }
        public string? LrCustom8 { get; set; }

        // 🔹 Company Details (NEW)
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyGstin { get; set; }
        public string? CompanyState { get; set; }
        public string? CompanyMobile { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyWebsite { get; set; }
        public string? CompanyPan { get; set; }
    }
}
