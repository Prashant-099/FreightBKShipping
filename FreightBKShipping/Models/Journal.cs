using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("journals")]
    public class Journal
    {
        [Key]
        [Column("journal_id")]
        public int JournalId { get; set; }

        [Column("journal_company_id")]
        public int JournalCompanyId { get; set; }

        [Column("journal_addedby_user_id")]
        public string? JournalAddedByUserId { get; set; }

        [Column("journal_updatedby_user_id")]
        public string? JournalUpdatedByUserId { get; set; }

        [Column("journal_voucher_id")]
        public int JournalVoucherId { get; set; }

        [Column("journal_year_id")]
        public int JournalYearId { get; set; }

        [Column("journal_party_id")]
        public int JournalPartyId { get; set; }

        [Column("journal_account_id")]
        public int JournalAccountId { get; set; }

        [Column("journal_no")]
        public int JournalNo { get; set; }

        [Column("journal_nostr")]
        public string? JournalNoStr { get; set; }

        [Column("journal_mastertype")]
        public string? JournalMasterType { get; set; }

        [Column("journal_date")]
        public DateTime JournalDate { get; set; }

        [Column("journal_amount")]
        public double JournalAmount { get; set; }

        [Column("journal_chqno")]
        public string? JournalChqNo { get; set; }

        [Column("journal_chqdate")]
        public string? JournalChqDate { get; set; }

        [Column("journal_remarks")]
        public string? JournalRemarks { get; set; }

        [Column("journal_prefix")]
        public string? JournalPrefix { get; set; }

        [Column("journal_postfix")]
        public string? JournalPostfix { get; set; }

        [Column("journal_refType")]
        public string? JournalRefType { get; set; }

        [Column("journal_status")]
        public bool JournalStatus { get; set; }

        [Column("journal_created")]
        public DateTime JournalCreated { get; set; }

        [Column("journal_updated")]
        public DateTime? JournalUpdated { get; set; }

        [Column("journal_totdis")]
        public float JournalTotalDiscount { get; set; }

        [Column("journal_totshort")]
        public float JournalTotalShort { get; set; }

        [Column("journal_tottds")]
        public float JournalTotalTds { get; set; }

        [Column("journal_total")]
        public double JournalTotal { get; set; }

        [Column("journal_onAccount")]
        public float JournalOnAccount { get; set; }

        [Column("journal_locked_by")]
        public string? JournalLockedBy { get; set; }

        [Column("journal_approved_by")]
        public int? JournalApprovedBy { get; set; }

        [Column("journal_bill_id")]
        public int? JournalBillId { get; set; }

        // ✅ Navigation Properties
        [ForeignKey("JournalPartyId")]
        public virtual Account? Party { get; set; }

        [ForeignKey("JournalAccountId")]
        public virtual Account? Account { get; set; }

        [ForeignKey("JournalVoucherId")]
        public virtual Voucher? Voucher { get; set; }

        [ForeignKey("JournalYearId")]
        public virtual Year? Year { get; set; }

        // ✅ Collection of Bill Reference Details
        public virtual ICollection<BillRefDetail>? BillRefDetails { get; set; }
    }
}