using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Column("transaction_company_id")]
        public int TransactionCompanyId { get; set; }

        [Column("transaction_voucher_id")]
        public int TransactionVoucherId { get; set; }

        [Column("transaction_voucher_type_id")]
        public int TransactionVoucherTypeId { get; set; }

        [Column("transaction_account_id")]
        public int TransactionAccountId { get; set; }

        [Column("transaction_amount")]
        public double TransactionAmount { get; set; }

        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [Column("transaction_remarks")]
        public string? TransactionRemarks { get; set; }

        [Column("transaction_status")]
        public bool TransactionStatus { get; set; }

        [Column("transaction_created")]
        public DateTime TransactionCreated { get; set; }

        [Column("transaction_updated")]
        public DateTime? TransactionUpdated { get; set; }

        [Column("transaction_voucher_no")]
        public string? TransactionVoucherNo { get; set; }

        [Column("transaction_vouchertype")]
        public string? TransactionVoucherType { get; set; }

        // Navigation
        public ICollection<TransactionDetail>? TransactionDetails { get; set; }
    }
}
