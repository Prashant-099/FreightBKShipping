using FreightBKShipping.DTOs.VoucherDetailsDto;
using System;
using System.Collections.Generic;

namespace FreightBKShipping.DTOs.VoucherDto
{
    public class VoucherCreateDto
    {
        public int VoucherCompanyId { get; set; }
        public string? VoucherAddedByUserId { get; set; }
        public string? VoucherUpdatedByUserId { get; set; }

        public string? VoucherGroup { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherMethod { get; set; } = "Manual"; // Automatic / Manual
        public string? VoucherTitle { get; set; }

        public bool VoucherIsDuplicate { get; set; }
        public string? VoucherPrinter { get; set; }
        public int? VoucherReportId { get; set; }
        public string? VoucherDeclaration { get; set; }

        public int? VoucherBank1 { get; set; }
        public int? VoucherBank2 { get; set; }
        public string? VoucherJurisdiction { get; set; }
        public string? VoucherRemarks { get; set; }

        public int VoucherCopies { get; set; } = 1;
        public bool VoucherIsPrint { get; set; }
        public bool VoucherIsShowPreview { get; set; }
        public bool VoucherStatus { get; set; } = true;

        public DateTime VoucherCreated { get; set; } = DateTime.UtcNow;
        public DateTime? VoucherUpdated { get; set; }

        public bool VoucherIsTaxInvoice { get; set; }
        public string? VoucherDetailLutno { get; set; }
        public string? VoucherReset { get; set; }
        public int? VoucherReportId2 { get; set; }

        public string VoucherCode { get; set; } = string.Empty;
        public bool VoucherIsPrintDialog { get; set; }
        public int? VoucherBranchId { get; set; }

        // 🔗 Child details
        public List<VoucherDetailCreateDto> VoucherDetails { get; set; } = new();
    }
}
