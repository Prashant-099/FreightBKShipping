﻿namespace FreightBKShipping.DTOs.BillDto
{
    public class BillDetailDto
    {

        public string? BillDetailAddedByUserId { get; set; } = string.Empty;
        public string? BillDetailUpdatedByUserId { get; set; } = string.Empty;
        public DateTime BillDetailCreated { get; set; } = DateTime.UtcNow;
        public DateTime? BillDetailUpdated { get; set; } = DateTime.UtcNow;
        public int BillDetailId { get; set; }
        public int BillDetailBillId { get; set; }
        public int BillDetailProductId { get; set; }
        public int BillDetailUnitId { get; set; }
        public int BillDetailHsnId { get; set; }
        public double BillDetailQty { get; set; }
        public double BillDetailRate { get; set; }
        public double BillDetailAmount { get; set; }
        public string? BillDetailRemarks { get; set; }
        public double BillDetailTotal { get; set; }
        public string? BillDetailUnit { get; set; }
        public string? BillDetailHsnCode { get; set; }
        public float BillDetailExtraChrg { get; set; }
        public int BillDetailCurrencyId { get; set; }
        public double BillDetailTaxableAmt { get; set; }
        public float BillDetailGstPer { get; set; }
        public double BillDetailIgst { get; set; }
        public float BillDetailCgstPer { get; set; }
        public double BillDetailCgst { get; set; }
        public float BillDetailIgstPer { get; set; }
        

        public double BillDetailSgst { get; set; }
        public float BillDetailSgstPer { get; set; }






    }
}
