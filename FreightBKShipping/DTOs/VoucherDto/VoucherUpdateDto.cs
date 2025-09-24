using FreightBKShipping.DTOs.VoucherDetailsDto;

namespace FreightBKShipping.DTOs.VoucherDto
{
    public class VoucherUpdateDto
    {
        public string? VoucherUpdatedByUserId { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherTitle { get; set; }
        public string? VoucherGroup { get; set; }
        public string? VoucherMethod { get; set; }
        public bool VoucherStatus { get; set; }

        public List<VoucherDetailUpdateDto>? VoucherDetails { get; set; }
    }
}
