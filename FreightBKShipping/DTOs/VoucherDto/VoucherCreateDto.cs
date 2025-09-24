using FreightBKShipping.DTOs.VoucherDetailsDto;
using System;
using System.ComponentModel.DataAnnotations;

namespace FreightBKShipping.DTOs
{
    public class VoucherCreateDto
    {
        public int VoucherCompanyId { get; set; }
        public string? VoucherAddedByUserId { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherTitle { get; set; }
        public string? VoucherGroup { get; set; }
        public string? VoucherMethod { get; set; }
        public bool VoucherStatus { get; set; } = true;

        public List<VoucherDetailCreateDto> VoucherDetails { get; set; } = new();
    }
}
