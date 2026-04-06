using FreightBKShipping.Controllers;
using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]


    public class AccountLedgerController : BaseController
    {
        private readonly AppDbContext _context;

        public AccountLedgerController(AppDbContext context)
        {
            _context = context;
        }

    [HttpGet]
    public async Task<IActionResult> GetAccountLedger(int accountId, DateTime? fromDate, DateTime? toDate)
    {
        var companyId = GetCompanyId();

        if (toDate.HasValue)
            toDate = toDate.Value.Date.AddDays(1).AddTicks(-1);

        // ── 1. Bills (Sales, Purchase, Debit Note, Credit Note) ──────────────
        var bills =
            from b in _context.Bills.Include(x => x.Party)
            where b.BillCompanyId == companyId
                  && b.BillPartyId == accountId
                  && b.BillStatus == true
                  && (fromDate == null || b.BillDate >= fromDate)
                  && (toDate == null || b.BillDate <= toDate)
                  && (b.BillType == null || !b.BillType.Trim().ToLower().Equals("quotation"))
            select new LedgerDto
            {
                VoucherDate = b.BillDate,
                VchName = b.BillType ?? "BILL",
                VoucherNo = b.BillNo,
                AccountName = b.partyname ?? (b.Party != null ? b.Party.AccountName : ""),
                Particulars = b.BillRemarks,
                Debit = b.BillType == "Sales" || b.BillType == "Debit Note"
                             ? (decimal)b.BillNetAmount : 0,
                Credit = b.BillType == "Purchase" || b.BillType == "Credit Note"
                             ? (decimal)b.BillNetAmount : 0
            };

        // ── 2. DR/CR Journal Adjustments (via BillRefDetail) ─────────────────
        // billref_against_id = JournalId, billref_accountid = affected account
        var journalAdjustments =
            from br in _context.BillRefDetails.Include(x => x.Account)
            join j in _context.Journals on br.BillRefAgainstId equals j.JournalId
            where (br.BillRefVchType == "DR" || br.BillRefVchType == "CR")
                  && (br.BillRefAccountId == accountId || br.BillRefVchId == accountId)
                  && j.JournalCompanyId == companyId
                  && (fromDate == null || j.JournalDate >= fromDate)
                  && (toDate == null || j.JournalDate <= toDate)
            select new LedgerDto
            {
                VoucherDate = j.JournalDate,
                VchName = j.JournalMasterType,
                VoucherNo = j.JournalNoStr,
                AccountName = j.JournalAccountId == accountId ? j.Account.AccountName : j.Party.AccountName,
                Particulars = j.JournalRemarks,
                Debit = br.BillRefVchType == "DR" ? (decimal)br.BillRefVchAmount : 0,
                Credit = br.BillRefVchType == "CR" ? (decimal)br.BillRefVchAmount : 0
            };

        // ── 3. RECEIPT/PAYMENT via BillRefDetail ─────────────────────────────
        // billref_against_id = JournalId (the receipt/payment journal)
        // billref_accountid  = party account (use this to filter, NOT JournalPartyId)
        // billref_vch_id     = Bill ID that was settled (not needed for ledger display)
        var receiptPaymentWithRef =
            from br in _context.BillRefDetails.Include(x => x.Account)
            join j in _context.Journals on br.BillRefAgainstId equals j.JournalId
            where (br.BillRefVchType == "RECEIPT" || br.BillRefVchType == "PAYMENT")
                  && (br.BillRefAccountId == accountId || j.JournalPartyId == accountId)    // ← party filter is HERE
                  && j.JournalCompanyId == companyId
                  && (fromDate == null || j.JournalDate >= fromDate)
                  && (toDate == null || j.JournalDate <= toDate)
            select new LedgerDto
            {
                VoucherDate = j.JournalDate,
                VchName = j.JournalMasterType,
                VoucherNo = j.JournalNoStr,
                AccountName = j.JournalAccountId == accountId ? j.Account.AccountName : j.Party.AccountName,
                Particulars = j.JournalRemarks,
                Debit = br.BillRefVchType == "PAYMENT" ? (decimal)br.BillRefVchAmount : 0,
                Credit = br.BillRefVchType == "RECEIPT" ? (decimal)br.BillRefVchAmount : 0
            };

        // ── 4. RECEIPT/PAYMENT with NO BillRefDetail at all ──────────────────
        // These are on-account payments — JournalPartyId holds the party here
        var receiptPaymentWithoutRef =
            from j in _context.Journals.Include(x => x.Account)
            where j.JournalCompanyId == companyId
                  && (j.JournalAccountId == accountId || j.JournalPartyId == accountId)          // on-account: party stored here
                  && j.JournalStatus == true
                  && (j.JournalMasterType == "RECEIPT" || j.JournalMasterType == "PAYMENT")
                  && !_context.BillRefDetails.Any(br => br.BillRefAgainstId == j.JournalId)
                  && (fromDate == null || j.JournalDate >= fromDate)
                  && (toDate == null || j.JournalDate <= toDate)
            select new LedgerDto
            {
                VoucherDate = j.JournalDate,
                VchName = j.JournalMasterType,
                VoucherNo = j.JournalNoStr,
                AccountName = j.JournalAccountId == accountId ? j.Account.AccountName : j.Party.AccountName,
                Particulars = j.JournalRemarks,
                Debit = j.JournalMasterType == "PAYMENT" ? (decimal)j.JournalTotal : 0,
                Credit = j.JournalMasterType == "RECEIPT" ? (decimal)j.JournalTotal : 0
            };

        // ── 5. Pure JOURNAL entries (no BillRefDetail) ───────────────────────
        var contraEntries =
    from j in _context.Journals.Include(x => x.Account)
    where j.JournalCompanyId == companyId
          && j.JournalStatus == true
          && j.JournalMasterType == "CONTRA"
          && (j.JournalAccountId == accountId || j.JournalPartyId == accountId)
          && !_context.BillRefDetails.Any(br => br.BillRefAgainstId == j.JournalId)
          && (fromDate == null || j.JournalDate >= fromDate)
          && (toDate == null || j.JournalDate <= toDate)

    select new LedgerDto
    {
        VoucherDate = j.JournalDate,
        VchName = j.JournalMasterType,
        VoucherNo = j.JournalNoStr,
        Particulars = j.JournalRemarks,
        AccountName = j.JournalAccountId == accountId? j.Account.AccountName:j.Party.AccountName ,
        Debit = j.JournalPartyId == accountId ? (decimal)j.JournalTotal : 0,
        Credit = j.JournalAccountId == accountId ? (decimal)j.JournalTotal : 0
    };
        // ── 6. TDS / Discount / Shortage Entries ───────────────────────
        var extraEntries =
            from j in _context.Journals
            where j.JournalCompanyId == companyId
                  && (j.TDSAcccountId == accountId
                      || j.DiscountAccountId == accountId
                      || j.ShortageAccountId == accountId)
                  && j.JournalStatus == true
                  && (fromDate == null || j.JournalDate >= fromDate)
                  && (toDate == null || j.JournalDate <= toDate)
            select new LedgerDto
            {
                VoucherDate = j.JournalDate,
                VchName = j.JournalMasterType,
                VoucherNo = j.JournalNoStr,
                Particulars = j.JournalRemarks,
                AccountName = j.Party.AccountName,

                Debit =
    j.JournalMasterType == "RECEIPT"
        ? (j.TDSAcccountId == accountId ? (decimal)j.JournalTotalTds :
           j.DiscountAccountId == accountId ? (decimal)j.JournalTotalDiscount :
           j.ShortageAccountId == accountId ? (decimal)j.JournalTotalShort : 0)
        : 0,

                Credit =
    j.JournalMasterType == "PAYMENT"
        ? (j.TDSAcccountId == accountId ? (decimal)j.JournalTotalTds :
           j.DiscountAccountId == accountId ? (decimal)j.JournalTotalDiscount :
           j.ShortageAccountId == accountId ? (decimal)j.JournalTotalShort : 0)
        : 0,
            };

        // ── Execute all queries ───────────────────────────────────────────────
        var billList = await bills.ToListAsync();
        var adjustmentList = await journalAdjustments.ToListAsync();
        var receiptWithRefList = await receiptPaymentWithRef.ToListAsync();
        var receiptWithoutRefList = await receiptPaymentWithoutRef.ToListAsync();
        var ContralList = await contraEntries.ToListAsync();
        var extraList = await extraEntries.ToListAsync();

        // ── Concat (NOT Union) to avoid silent deduplication ─────────────────
        var ledger = billList
            .Concat(adjustmentList)
            .Concat(receiptWithRefList)
            .Concat(receiptWithoutRefList)
            .Concat(ContralList)
            .Concat(extraList)
            .OrderBy(x => x.VoucherDate)
            .ThenBy(x => x.VoucherNo)
            .ToList();

        return Ok(ledger);
    }


}

