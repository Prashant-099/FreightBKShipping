namespace FreightBKShipping.DTOs.BillDto
{
    public class BillDetailDto
    {
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
    }
}
