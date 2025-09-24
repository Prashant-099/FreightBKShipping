namespace FreightBKShipping.DTOs.VoucherDetailsDto
{
    public class VoucherDetailCreateDto
    {
        public int VoucherDetailYearId { get; set; }
        public int VoucherDetailStartNo { get; set; }
        public string? VoucherDetailPrefix { get; set; }
        public string? VoucherDetailSufix { get; set; }
        public int VoucherDetailZeroFill { get; set; }
        public bool VoucherDetailStatus { get; set; } = true;
    }
}
