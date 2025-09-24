using FreightBKShipping.DTOs.VoucherDetailsDto;
namespace FreightBKShipping.DTOs.VoucherDto
{
    public class VoucherReadDto
    {
        public int VoucherId { get; set; }
        public int VoucherCompanyId { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherTitle { get; set; }
        public string? VoucherGroup { get; set; }
        public string? VoucherMethod { get; set; }
        public bool VoucherStatus { get; set; }
        public DateTime VoucherCreated { get; set; }
        public DateTime? VoucherUpdated { get; set; }

        public List<VoucherDetailReadDto> VoucherDetails { get; set; } = new();
    }
}
