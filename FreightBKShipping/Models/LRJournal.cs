using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
        [Table("lr_journal")]
        public class LRJournal
        {
            [Key]
            [Column("lr_journal_id")]
            public int LrJournalId { get; set; }

            [Column("lr_journal_addedby_user_id")]
            [StringLength(30)]
            public string? AddedByUserId { get; set; }

            [Column("lr_journal_updatedby_user_id")]
            [StringLength(30)]
            public string? UpdatedByUserId { get; set; }

            [Column("lr_journal_lr_id")]
            public int? LrId { get; set; }

            [Column("lr_journal_no")]
            [StringLength(50)]
            public string? JournalNo { get; set; }

            [Column("lr_journal_date")]
            public DateTime? JournalDate { get; set; }

            [Column("lr_journal_amount")]
            public float? Amount { get; set; }

            [Column("lr_journal_urea_qty")]
            public float? UreaQty { get; set; } = 0;

            [Column("lr_journal_petrocash")]
            public float? PetroCash { get; set; } = 0;

            [Column("lr_journal_rate")]
            public float? Rate { get; set; }

            [Column("lr_journal_qty")]
            public float? Qty { get; set; }

            [Column("lr_journal_slipno")]
            [StringLength(50)]
            public string? SlipNo { get; set; }

            [Column("lr_journal_remarks")]
            [StringLength(300)]
            public string? Remarks { get; set; }

            [Column("lr_journal_group")]
            public LrJournalGroup? JournalGroup { get; set; }

            [Column("lr_journal_vchtype")]
            public LrJournalVchType? VchType { get; set; }

            [Column("lr_journal_type")]
            public LrJournalType? JournalType { get; set; }

            [Column("lr_journal_party_id")]
            public int? PartyId { get; set; }

            [Column("lr_journal_account_id")]
            public int? AccountId { get; set; }

            [Column("lr_journal_cheqno")]
            [StringLength(30)]
            public string? ChequeNo { get; set; }

            [Column("lr_journal_cheqe_date")]
            [StringLength(45)]
            public string? ChequeDate { get; set; }

            [Required]
            [Column("lr_journal_status")]
            public bool Status { get; set; } = true;

            [Column("lr_journal_created")]
            public DateTime Created { get; set; }

            [Column("lr_journal_updated")]
            public DateTime Updated { get; set; }

            [Column("lr_journal_paidby_id")]
            public int? PaidById { get; set; }

            [Column("lr_journal_sno")]
            public int Sno { get; set; }

            [Column("lr_journal_adddeduct")]
            [StringLength(45)]
            public string? AddDeduct { get; set; } = "ADD";

            [Column("lr_journal_adv_type")]
            [StringLength(45)]
            public string? AdvType { get; set; } = "OTHER";

            [Column("lr_journal_default_remarks")]
            [StringLength(500)]
            public string? DefaultRemarks { get; set; }

            [Column("lr_journal_company_id")]
            [StringLength(45)]
            public string? CompanyId { get; set; }

            [Column("lr_journal_year_id")]
            [StringLength(45)]
            public string? YearId { get; set; }

            [Column("lr_journal_trip_date")]
            public DateTime? TripDate { get; set; }

            [Column("lr_journal_prefix")]
            [StringLength(45)]
            public string? Prefix { get; set; }

            [Column("lr_journal_sufix")]
            [StringLength(45)]
            public string? Suffix { get; set; }

            [Column("lr_journal_vchtype_id")]
            public int? VchTypeId { get; set; }

            [Column("lr_journal_vehicle_id")]
            [StringLength(45)]
            public string? VehicleId { get; set; }

            [Column("lr_journal_expense_id")]
            public int? ExpenseId { get; set; } = 0;

            [Column("lr_journal_pump_billid")]
            public int? PumpBillId { get; set; } = 0;

            [Column("lr_journal_urea_amt")]
            public double? UreaAmount { get; set; } = 0;

            [Column("lr_journal_urea_rate")]
            public double? UreaRate { get; set; } = 0;

            [Column("lr_journal_trans_id")]
            [StringLength(50)]
            public string? TransactionId { get; set; } = "";

        [ForeignKey("LrId")]
        public Lr? Lr { get; set; }
    }

    public enum LrJournalGroup
    {
        Advance,
        Charges,
        Expense,
        AdvanceReceived,
        Diesel
    }


    public enum LrJournalVchType
        {
            Receipt,
            Payment,
            Journal,
            DebitNote,
            CreditNote,
            Sales,
            Purchase
        }

        public enum LrJournalType
        {
            CUSTOMER,
            VEHICLE
        }
    
}
