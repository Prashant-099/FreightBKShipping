using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{

    [Table("audit_logs")]
    public class AuditLog
    {
        [Key]
        [Column("audit_logs_id")]
        public int AuditLogsId { get; set; }

        [Column("table_name")]
        public string TableName { get; set; }

        [Column("record_id")]
        public int RecordId { get; set; }

        [Column("voucher_type")]
        public string VoucherType { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("operations")]
        public string Operations { get; set; }

        [Column("remarks")]
        public string Remarks { get; set; }

        [Column("date_time")]
        public DateTime DateTime { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("company_id")]
        public int CompanyId { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [Column("year_id")]
        public int YearId { get; set; }
    }
}
