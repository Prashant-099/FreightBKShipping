﻿using System;
using System.Collections.Generic;
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
        public bool VoucherStatus { get; set; }

        public DateTime? VoucherUpdated { get; set; }

        public bool VoucherIsTaxInvoice { get; set; }
        public string? VoucherDetailLutno { get; set; }
        public string? VoucherReset { get; set; }
        public int? VoucherReportId2 { get; set; }

        public string VoucherCode { get; set; } = string.Empty;
        public bool VoucherIsPrintDialog { get; set; }
        public int? VoucherBranchId { get; set; }

        // 🔗 Child details
        public List<VoucherDetailUpdateDto>? VoucherDetails { get; set; }
    }
}
