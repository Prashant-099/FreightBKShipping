using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("transaction_details")]
    public class TransactionDetail
    {
        [Key]
        [Column("transaction_detail_id")]
        public int TransactionDetailId { get; set; }

        [Column("transaction_detail_company_id")]
        public int TransactionDetailCompanyId { get; set; }

        [Column("transaction_detail_transaction_id")]
        public int TransactionDetailTransactionId { get; set; }

        [Column("transaction_detail_party_id")]
        public int TransactionDetailPartyId { get; set; }

        [Column("transaction_detail_account_id")]
        public int TransactionDetailAccountId { get; set; }

        [Column("transaction_detail_cr")]
        public double Credit { get; set; }

        [Column("transaction_detail_dr")]
        public double Debit { get; set; }

        [Column("transaction_detail_status")]
        public bool TransactionDetailStatus { get; set; }

        [Column("transaction_detail_created")]
        public DateTime TransactionDetailCreated { get; set; }

        [Column("transaction_detail_updated")]
        public DateTime? TransactionDetailUpdated { get; set; }

        // Navigation
        [ForeignKey("TransactionDetailTransactionId")]
        public Transaction? Transaction { get; set; }

        [ForeignKey("TransactionDetailAccountId")]
        public Account? Account { get; set; }

        [ForeignKey("TransactionDetailPartyId")]
        public Account? Party { get; set; }
    }
}
