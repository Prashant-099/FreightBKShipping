using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("vouchers")]
    public class Voucher
    {
        [Key]
        [Column("voucher_id")]
        public int VoucherId { get; set; }

        [Column("voucher_company_id")]
        public int VoucherCompanyId { get; set; }

        [Column("voucher_addedby_user_id")]
        public string? VoucherAddedByUserId { get; set; }

        [Column("voucher_updatedby_user_id")]
        public string? VoucherUpdatedByUserId { get; set; }

        [Column("voucher_group")]
       
        public string VoucherGroup { get; set; } = string.Empty;

        [Column("voucher_name")]
        
        public string VoucherName { get; set; } = string.Empty;

        [Column("voucher_method")]
       
        public string VoucherMethod { get; set; } = "Manual"; // "Automatic" / "Manual"

        [Column("voucher_title")]
       
        public string? VoucherTitle { get; set; }

        [Column("voucher_isduplicate")]
        public bool VoucherIsDuplicate { get; set; }

        [Column("voucher_printer")]
      
        public string? VoucherPrinter { get; set; }

        [Column("voucher_report_id")]
        public int? VoucherReportId { get; set; }

        [Column("voucher_declaration")]
     
        public string? VoucherDeclaration { get; set; }

        [Column("voucher_bank1")]
        public int? VoucherBank1 { get; set; }

        [Column("voucher_bank2")]
        public int? VoucherBank2 { get; set; }

        [Column("voucher_jurisdiction")]
   
        public string? VoucherJurisdiction { get; set; }

        [Column("voucher_remarks")]
  
        public string? VoucherRemarks { get; set; }

        [Column("voucher_copies")]
        public int VoucherCopies { get; set; } = 1;

        [Column("voucher_isprint")]
        public bool VoucherIsPrint { get; set; }

        [Column("voucher_isshowpreview")]
        public bool VoucherIsShowPreview { get; set; }

        [Column("voucher_status")]
        public bool VoucherStatus { get; set; }

        [Column("voucher_created")]
        public DateTime VoucherCreated { get; set; } = DateTime.UtcNow;

        [Column("voucher_updated")]
        public DateTime? VoucherUpdated { get; set; }

        [Column("voucher_istaxinvoice")]
        public bool VoucherIsTaxInvoice { get; set; }

        [Column("voucher_detail_lutno")]
     
        public string? VoucherDetailLutno { get; set; }

        [Column("voucher_reset")]

        public string? VoucherReset { get; set; }

        [Column("voucher_report_id2")]
        public int? VoucherReportId2 { get; set; }

        [Column("voucher_code")]

        public string VoucherCode { get; set; } = string.Empty;

        [Column("voucher_isprintdialog")]
        public bool VoucherIsPrintDialog { get; set; }

        [Column("voucher_branch_id")]
        public int? VoucherBranchId { get; set; }

        // 🔗 Navigation
        public ICollection<VoucherDetail> VoucherDetails { get; set; } = new List<VoucherDetail>();
    }
}
