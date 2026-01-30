using System;
using System.Collections.Generic;

namespace FreightBKShipping.DTOs.JournalDto
{
    // Main Journal DTO
    public class JournalDto
    {
        public int JournalId { get; set; }
        public int JournalCompanyId { get; set; }
        public string? JournalAddedByUserId { get; set; }
        public string? JournalUpdatedByUserId { get; set; }
        public int JournalVoucherId { get; set; }
        public int JournalYearId { get; set; }
        public int JournalPartyId { get; set; }
        public int JournalAccountId { get; set; }
        public int JournalNo { get; set; }
        public string? JournalNoStr { get; set; }
        public string? JournalMasterType { get; set; }
        public DateTime JournalDate { get; set; }
        public double JournalAmount { get; set; }
        public string? JournalChqNo { get; set; }
        public string? JournalChqDate { get; set; }
        public string? JournalRemarks { get; set; }
        public string? JournalPrefix { get; set; }
        public string? JournalPostfix { get; set; }
        public string? JournalRefType { get; set; }
        public bool JournalStatus { get; set; }
        public DateTime JournalCreated { get; set; }
        public DateTime? JournalUpdated { get; set; }
        public float JournalTotalDiscount { get; set; }
        public float JournalTotalShort { get; set; }
        public float JournalTotalTds { get; set; }
        public double JournalTotal { get; set; }
        public float JournalOnAccount { get; set; }
        public string? JournalLockedBy { get; set; }
        public string? JournalLockedByUsername { get; set; }
        public int? JournalApprovedBy { get; set; }
        public int? JournalBillId { get; set; }

        // Navigation properties (read-only)
        public string? PartyName { get; set; }
        public string? AccountName { get; set; }
        public string? VoucherName { get; set; }

        // Child details
        public List<BillRefDetailDto>? BillRefDetails { get; set; }
    }

    // Bill Reference Detail DTO
    public class BillRefDetailDto
    {
        public int BillRefDetailId { get; set; }
        public int BillRefAgainstId { get; set; }
        public string? BillRefVchType { get; set; }
        public string? BillRefVchNo { get; set; }
        public int BillRefVchId { get; set; }
        public DateTime BillRefVchDate { get; set; }
        public double BillRefVchAmount { get; set; }
        public float BillRefVchDis { get; set; }
        public float BillRefVchTds { get; set; }
        public float BillRefVchShort { get; set; }
        public float BillRefVchBalance { get; set; }
        public int BillRefAccountId { get; set; }
    }
}