using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("voucher_details")]
    public class VoucherDetail
    {
        [Key]
        [Column("voucher_detail_id")]
        public int VoucherDetailId { get; set; }

        [Column("voucher_detail_voucher_id")]
        public int VoucherDetailVoucherId { get; set; }

        [Column("voucher_detail_year_id")]
        public int VoucherDetailYearId { get; set; }

        [Column("voucher_detail_startno")]
        public int VoucherDetailStartNo { get; set; }

        [Column("voucher_detail_width")]
  
        public string VoucherDetailWidth { get; set; } = string.Empty;

        [Column("voucher_detail_prefix")]
       
        public string VoucherDetailPrefix { get; set; } = string.Empty;

        [Column("voucher_detail_sufix")]
    
        public string VoucherDetailSufix { get; set; } = string.Empty;

        [Column("voucher_detail_zerofill")]
        public int VoucherDetailZeroFill { get; set; }

        [Column("voucher_detail_isduplicate")]
        public bool VoucherDetailIsDuplicate { get; set; }

        [Column("voucher_detail_status")]
        public bool VoucherDetailStatus { get; set; }

        [Column("voucher_detail_created")]
        public DateTime VoucherDetailCreated { get; set; } = DateTime.UtcNow;

        [Column("voucher_detail_updated")]
        public DateTime? VoucherDetailUpdated { get; set; }

        [Column("voucher_detail_lutno")]
       
        public string? VoucherDetailLutno { get; set; }

        [Column("voucher_detail_date")]
        public DateTime? VoucherDetailDate { get; set; }

        [Column("voucher_detail_lastno")]
        public int VoucherDetailLastNo { get; set; }

        // 🔗 Navigation
        [ForeignKey("VoucherDetailVoucherId")]
        public Voucher? Voucher { get; set; }
    }
}
