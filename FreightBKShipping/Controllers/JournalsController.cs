using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.DTOs.JournalDto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;

        public JournalsController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
            _context = context;
        }   

        // GET: api/Journals
        [HttpGet]
        public async Task<ActionResult<List<JournalDto>>> GetAllJournals()
        {
            var journals = await FilterByCompany(_context.Journals, "JournalCompanyId")
                .Where(j => j.JournalStatus == true)
                .OrderByDescending(j => j.JournalId)
                .ThenByDescending(j => j.JournalDate)
                .AsNoTracking()
                .Include(j => j.Party)
                .Include(j => j.Account)
                .Include(j => j.Voucher)
                .Include(j => j.BillRefDetails)
                .ToListAsync();

            // Get locked by usernames
            var lockedByUserIds = journals
                .Where(j => !string.IsNullOrEmpty(j.JournalLockedBy))
                .Select(j => j.JournalLockedBy)
                .Distinct()
                .ToList();

            var userNames = await _context.Users
                .Where(u => lockedByUserIds.Contains(u.UserId))
                .ToDictionaryAsync(u => u.UserId, u => u.UserName);

            var result = journals.Select(j => new JournalDto
            {
                JournalId = j.JournalId,
                JournalCompanyId = j.JournalCompanyId,
                JournalAddedByUserId = j.JournalAddedByUserId,
                JournalUpdatedByUserId = j.JournalUpdatedByUserId,
                JournalVoucherId = j.JournalVoucherId,
                JournalYearId = j.JournalYearId,
                JournalPartyId = j.JournalPartyId,
                JournalAccountId = j.JournalAccountId,
                JournalNo = j.JournalNo,
                JournalNoStr = j.JournalNoStr,
                JournalMasterType = j.JournalMasterType,
                JournalDate = j.JournalDate,
                JournalAmount = j.JournalAmount,
                JournalChqNo = j.JournalChqNo,
                JournalChqDate = j.JournalChqDate,
                JournalRemarks = j.JournalRemarks,
                JournalPrefix = j.JournalPrefix,
                JournalPostfix = j.JournalPostfix,
                JournalRefType = j.JournalRefType,
                JournalStatus = j.JournalStatus,
                JournalCreated = j.JournalCreated,
                JournalUpdated = j.JournalUpdated,
                JournalTotalDiscount = j.JournalTotalDiscount,
                JournalTotalShort = j.JournalTotalShort,
                JournalTotalTds = j.JournalTotalTds,
                JournalTotal = j.JournalTotal,
                JournalOnAccount = j.JournalOnAccount,
                JournalLockedBy = j.JournalLockedBy,
                JournalLockedByUsername = !string.IsNullOrEmpty(j.JournalLockedBy) && userNames.ContainsKey(j.JournalLockedBy)
                    ? userNames[j.JournalLockedBy]
                    : null,
                JournalApprovedBy = j.JournalApprovedBy,
                JournalBillId = j.JournalBillId,
                PartyName = j.Party?.AccountName,
                AccountName = j.Account?.AccountName,
                VoucherName = j.Voucher?.VoucherName,
           // ✅ BILL REF SAME AS BILL DETAILS
        BillRefDetails = j.BillRefDetails
            .OrderBy(r => r.BillRefDetailId)
            .Select(r => new BillRefDetailDto
            {
                BillRefDetailId = r.BillRefDetailId,
                BillRefAgainstId = r.BillRefAgainstId,
                BillRefVchType = r.BillRefVchType,
                BillRefVchNo = r.BillRefVchNo,
                BillRefVchId = r.BillRefVchId,
                BillRefVchDate = r.BillRefVchDate,
                BillRefVchAmount = r.BillRefVchAmount,
                BillRefVchDis = r.BillRefVchDis,
                BillRefVchTds = r.BillRefVchTds,
                BillRefVchShort = r.BillRefVchShort,
                BillRefVchBalance = r.BillRefVchBalance,
                BillRefAccountId = r.BillRefAccountId
            }).ToList()
            })
    .OrderByDescending(j => j.JournalId)
    .ToList();

            return Ok(result);
        }

        [HttpPost("{id}/lock")]
        public async Task<IActionResult> ToggleJournalLock(int id)
        {
            var journal = await FilterByCompany(_context.Journals, "JournalCompanyId")
                .FirstOrDefaultAsync(j => j.JournalId == id);

            if (journal == null)
                return NotFound();

            // Toggle lock
            if (string.IsNullOrEmpty(journal.JournalLockedBy))
            {
                journal.JournalLockedBy = GetUserId();
            }
            else
            {
                journal.JournalLockedBy = "";
            }

            journal.JournalUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { locked = !string.IsNullOrEmpty(journal.JournalLockedBy), lockedBy = journal.JournalLockedBy });
        }

        // GET: api/Journals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JournalDto>> GetJournal(int id)
        {
            var journal = await FilterByCompany(_context.Journals, "JournalCompanyId")
                .Include(j => j.BillRefDetails)
                .Include(j => j.Party)
                .Include(j => j.Account)
                .Include(j => j.Voucher)
                .FirstOrDefaultAsync(j => j.JournalId == id);

            if (journal == null) return NotFound();

            var dto = new JournalDto
            {
                JournalId = journal.JournalId,
                JournalCompanyId = journal.JournalCompanyId,
                JournalAddedByUserId = journal.JournalAddedByUserId,
                JournalUpdatedByUserId = journal.JournalUpdatedByUserId,
                JournalVoucherId = journal.JournalVoucherId,
                JournalYearId = journal.JournalYearId,
                JournalPartyId = journal.JournalPartyId,
                JournalAccountId = journal.JournalAccountId,
                JournalNo = journal.JournalNo,
                JournalNoStr = journal.JournalNoStr,
                JournalMasterType = journal.JournalMasterType,
                JournalDate = journal.JournalDate,
                JournalAmount = journal.JournalAmount,
                JournalChqNo = journal.JournalChqNo,
                JournalChqDate = journal.JournalChqDate,
                JournalRemarks = journal.JournalRemarks,
                JournalPrefix = journal.JournalPrefix,
                JournalPostfix = journal.JournalPostfix,
                JournalRefType = journal.JournalRefType,
                JournalStatus = journal.JournalStatus,
                JournalCreated = journal.JournalCreated,
                JournalUpdated = journal.JournalUpdated,
                JournalTotalDiscount = journal.JournalTotalDiscount,
                JournalTotalShort = journal.JournalTotalShort,
                JournalTotalTds = journal.JournalTotalTds,
                JournalTotal = journal.JournalTotal,
                JournalOnAccount = journal.JournalOnAccount,
                JournalLockedBy = journal.JournalLockedBy,
                JournalApprovedBy = journal.JournalApprovedBy,
                JournalBillId = journal.JournalBillId,
                PartyName = journal.Party?.AccountName,
                AccountName = journal.Account?.AccountName,
                VoucherName = journal.Voucher?.VoucherName,
                BillRefDetails = journal.BillRefDetails?.Select(r => new BillRefDetailDto
                {
                    BillRefDetailId = r.BillRefDetailId,
                    BillRefAgainstId = r.BillRefAgainstId,
                    BillRefVchType = r.BillRefVchType,
                    BillRefVchNo = r.BillRefVchNo,
                    BillRefVchId = r.BillRefVchId,
                    BillRefVchDate = r.BillRefVchDate,
                    BillRefVchAmount = r.BillRefVchAmount,
                    BillRefVchDis = r.BillRefVchDis,
                    BillRefVchTds = r.BillRefVchTds,
                    BillRefVchShort = r.BillRefVchShort,
                    BillRefVchBalance = r.BillRefVchBalance,
                    BillRefAccountId = r.BillRefAccountId
                }).ToList()
            };

            return dto;
        }

        // POST: api/Journals
        [HttpPost]
        public async Task<ActionResult<Journal>> CreateJournal(JournalDto journalDto)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get voucher
                var voucher = await _context.Vouchers
                    .FirstOrDefaultAsync(v => v.VoucherId == journalDto.JournalVoucherId &&
                                            v.VoucherCompanyId == GetCompanyId());

                if (voucher == null)
                    return BadRequest(new { message = "Invalid voucher selected." });

                bool isAutomatic = voucher.VoucherMethod == "Automatic";

                // Get voucher detail for the year
                var voucherDetail = await _context.VoucherDetails
                    .FirstOrDefaultAsync(vd =>
                        vd.VoucherDetailVoucherId == voucher.VoucherId &&
                        vd.VoucherDetailYearId == journalDto.JournalYearId &&
                        vd.VoucherDetailStatus == true);

                if (voucherDetail == null)
                    return BadRequest(new { message = "Voucher details not configured." });

                int nextNo;
                string journalNoStr;

                if (isAutomatic)
                {
                    // Auto number
                    nextNo = voucherDetail.VoucherDetailLastNo + 1;

                    bool exists = await _context.Journals.AnyAsync(j =>
                        j.JournalNo == nextNo &&
                        j.JournalCompanyId == GetCompanyId() &&
                        j.JournalVoucherId == journalDto.JournalVoucherId &&
                        j.JournalYearId == journalDto.JournalYearId &&
                        j.JournalStatus == true);

                    if (exists)
                    {
                        nextNo = (await _context.Journals
                            .Where(j => j.JournalCompanyId == GetCompanyId() &&
                                        j.JournalVoucherId == journalDto.JournalVoucherId &&
                                        j.JournalStatus == true &&
                                        j.JournalYearId == journalDto.JournalYearId)
                            .MaxAsync(j => (int?)j.JournalNo) ?? nextNo) + 1;
                    }

                    journalNoStr =
                        (voucherDetail.VoucherDetailPrefix ?? "") +
                        nextNo +
                        (voucherDetail.VoucherDetailSufix ?? "");

                    voucherDetail.VoucherDetailLastNo = nextNo;
                    voucherDetail.VoucherDetailUpdated = DateTime.UtcNow;
                }
                else
                {
                    // Manual number
                    bool exists = await _context.Journals.AnyAsync(j =>
                        j.JournalNoStr == journalDto.JournalNoStr &&
                        j.JournalStatus == true &&
                        j.JournalCompanyId == GetCompanyId() &&
                        j.JournalVoucherId == journalDto.JournalVoucherId &&
                        j.JournalYearId == journalDto.JournalYearId);

                    if (exists)
                        return Conflict(new { message = "Journal number already exists." });

                    nextNo = 0;
                    journalNoStr = journalDto.JournalNoStr;
                }

                journalDto.JournalNo = nextNo;
                journalDto.JournalNoStr = journalNoStr;

                // Get party and account
                var party = await _context.Accounts
                    .FirstOrDefaultAsync(p => p.AccountId == journalDto.JournalPartyId);

                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountId == journalDto.JournalAccountId);

                var journal = new Journal
                {
                    JournalCompanyId = GetCompanyId(),
                    JournalAddedByUserId = GetUserId(),
                    JournalUpdatedByUserId = GetUserId(),
                    JournalCreated = DateTime.UtcNow,
                    JournalUpdated = DateTime.UtcNow,
                    JournalVoucherId = journalDto.JournalVoucherId,
                    JournalYearId = journalDto.JournalYearId,
                    JournalPartyId = journalDto.JournalPartyId,
                    JournalAccountId = journalDto.JournalAccountId,
                    JournalNo = nextNo,
                    JournalNoStr = journalNoStr,
                    JournalMasterType = journalDto.JournalMasterType,
                    JournalDate = journalDto.JournalDate,
                    JournalAmount = journalDto.JournalAmount,
                    JournalChqNo = journalDto.JournalChqNo,
                    JournalChqDate = journalDto.JournalChqDate,
                    JournalRemarks = journalDto.JournalRemarks,
                    JournalPrefix = journalDto.JournalPrefix,
                    JournalPostfix = journalDto.JournalPostfix,
                    JournalRefType = journalDto.JournalRefType,
                    JournalStatus = true,
                    JournalTotalDiscount = journalDto.JournalTotalDiscount,
                    JournalTotalShort = journalDto.JournalTotalShort,
                    JournalTotalTds = journalDto.JournalTotalTds,
                    JournalTotal = journalDto.JournalTotal,
                    JournalOnAccount = journalDto.JournalOnAccount,
                    JournalLockedBy = journalDto.JournalLockedBy,
                    JournalApprovedBy = journalDto.JournalApprovedBy,
                    JournalBillId = journalDto.JournalBillId,

                    // Bill Ref Details
                    BillRefDetails = journalDto.BillRefDetails?.Select(r => new BillRefDetail
                    {
                        BillRefAgainstId = r.BillRefAgainstId,
                        BillRefVchType = r.BillRefVchType,
                        BillRefVchNo = r.BillRefVchNo,
                        BillRefVchId = r.BillRefVchId,
                        BillRefVchDate = r.BillRefVchDate,
                        BillRefVchAmount = r.BillRefVchAmount,
                        BillRefVchDis = r.BillRefVchDis,
                        BillRefVchTds = r.BillRefVchTds,
                        BillRefVchShort = r.BillRefVchShort,
                        BillRefVchBalance = r.BillRefVchBalance,
                        BillRefAccountId = r.BillRefAccountId
                    }).ToList()
                };
                if (journalDto.BillRefDetails != null && journalDto.BillRefDetails.Any())
                {
                    foreach (var refDetail in journalDto.BillRefDetails)
                    {
                        var bill = await _context.Bills
                            .FirstOrDefaultAsync(b =>
                                b.BillId == refDetail.BillRefVchId &&
                                b.BillStatus == true &&
                                b.BillCompanyId == GetCompanyId());

                        if (bill == null)
                            continue;

                        // 🔹 Already settled from DB (OLD receipts)
                        var dbSettled = await _context.BillRefDetails
                            .Where(r => r.BillRefVchId == bill.BillId)
                            .SumAsync(r =>
                                r.BillRefVchAmount +
                                r.BillRefVchDis +
                                r.BillRefVchTds +
                                r.BillRefVchShort);

                        // 🔹 Current receipt settlement (NOT yet in DB)
                        var currentSettlement =
                            refDetail.BillRefVchAmount +
                            refDetail.BillRefVchDis +
                            refDetail.BillRefVchTds +
                            refDetail.BillRefVchShort;

                        var totalSettled = dbSettled + currentSettlement;

                        bill.Bill_due_amt = Math.Round(
                            Math.Max(bill.BillNetAmount - totalSettled, 0),
                            2
                        );

                        bill.BillUpdated = DateTime.UtcNow;
                    }
                }

                _context.Journals.Add(journal);
                await _context.SaveChangesAsync();

                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "Journals",
                    RecordId = journal.JournalId,
                    VoucherType = voucher.VoucherName,
                    Amount = (int)journal.JournalTotal,
                    Operations = "INSERT",
                    Remarks = voucher.VoucherName + " Journal No: " + journal.JournalNoStr,
                    YearId = journal.JournalYearId
                }, GetCompanyId());

                await tx.CommitAsync();
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                await tx.RollbackAsync();
                return BadRequest(new
                {
                    error = "Database update error",
                    details = dbEx.InnerException?.Message ?? dbEx.Message,
                    stack = dbEx.StackTrace
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return BadRequest(new { error = "Error creating journal", details = ex.Message, stack = ex.StackTrace });
            }
        }

        // PUT: api/Journals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJournal(int id, JournalDto journalDto)
        {
            if (id != journalDto.JournalId)
                return BadRequest("Journal ID mismatch.");

            var journal = await _context.Journals
                .Include(j => j.BillRefDetails)
                .FirstOrDefaultAsync(j => j.JournalId == id);

            if (journal == null) return NotFound();

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(v => v.VoucherId == journalDto.JournalVoucherId);

            if (voucher != null && voucher.VoucherMethod == "Manual")
            {
                bool exists = await _context.Journals.AnyAsync(j =>
                    j.JournalNoStr == journalDto.JournalNoStr &&
                    j.JournalStatus == true &&
                    j.JournalCompanyId == GetCompanyId() &&
                    j.JournalYearId == journalDto.JournalYearId &&
                    j.JournalId != id);

                if (exists)
                    return Conflict(new { message = "Journal number already exists." });
            }

            // Check if locked
            if (!string.IsNullOrEmpty(journal.JournalLockedBy) && journal.JournalLockedBy != GetUserId())
            {
                return BadRequest("This journal is locked by another user and cannot be edited.");
            }

            // Update main journal fields
            journal.JournalUpdatedByUserId = GetUserId();
            journal.JournalUpdated = DateTime.UtcNow;
            journal.JournalVoucherId = journalDto.JournalVoucherId;
            journal.JournalYearId = journalDto.JournalYearId;
            journal.JournalPartyId = journalDto.JournalPartyId;
            journal.JournalAccountId = journalDto.JournalAccountId;
            journal.JournalNo = journalDto.JournalNo;
            journal.JournalNoStr = journalDto.JournalNoStr;
            journal.JournalMasterType = journalDto.JournalMasterType;
            journal.JournalDate = journalDto.JournalDate;
            journal.JournalAmount = journalDto.JournalAmount;
            journal.JournalChqNo = journalDto.JournalChqNo;
            journal.JournalChqDate = journalDto.JournalChqDate;
            journal.JournalRemarks = journalDto.JournalRemarks;
            journal.JournalPrefix = journalDto.JournalPrefix;
            journal.JournalPostfix = journalDto.JournalPostfix;
            journal.JournalRefType = journalDto.JournalRefType;
            journal.JournalStatus = journalDto.JournalStatus;
            journal.JournalTotalDiscount = journalDto.JournalTotalDiscount;
            journal.JournalTotalShort = journalDto.JournalTotalShort;
            journal.JournalTotalTds = journalDto.JournalTotalTds;
            journal.JournalTotal = journalDto.JournalTotal;
            journal.JournalOnAccount = journalDto.JournalOnAccount;
            journal.JournalApprovedBy = journalDto.JournalApprovedBy;
            journal.JournalBillId = journalDto.JournalBillId;

            // Handle BillRefDetails - soft delete
            var refsToRemove = journal.BillRefDetails
                .Where(r => journalDto.BillRefDetails == null ||
                           !journalDto.BillRefDetails.Any(x => x.BillRefDetailId == r.BillRefDetailId))
                .ToList();
            _context.BillRefDetails.RemoveRange(refsToRemove);

            // Add or update BillRefDetails
            if (journalDto.BillRefDetails != null)
            {
                foreach (var refDto in journalDto.BillRefDetails)
                {
                    var existingRef = journal.BillRefDetails?.FirstOrDefault(r => r.BillRefDetailId == refDto.BillRefDetailId);
                    if (existingRef != null)
                    {
                        existingRef.BillRefAgainstId = refDto.BillRefAgainstId;
                        existingRef.BillRefVchType = refDto.BillRefVchType;
                        existingRef.BillRefVchNo = refDto.BillRefVchNo;
                        existingRef.BillRefVchId = refDto.BillRefVchId;
                        existingRef.BillRefVchDate = refDto.BillRefVchDate;
                        existingRef.BillRefVchAmount = refDto.BillRefVchAmount;
                        existingRef.BillRefVchDis = refDto.BillRefVchDis;
                        existingRef.BillRefVchTds = refDto.BillRefVchTds;
                        existingRef.BillRefVchShort = refDto.BillRefVchShort;
                        existingRef.BillRefVchBalance = refDto.BillRefVchBalance;
                        existingRef.BillRefAccountId = refDto.BillRefAccountId;
                    }
                    else
                    {
                        journal.BillRefDetails.Add(new BillRefDetail
                        {
                            BillRefAgainstId = refDto.BillRefAgainstId,
                            BillRefVchType = refDto.BillRefVchType,
                            BillRefVchNo = refDto.BillRefVchNo,
                            BillRefVchId = refDto.BillRefVchId,
                            BillRefVchDate = refDto.BillRefVchDate,
                            BillRefVchAmount = refDto.BillRefVchAmount,
                            BillRefVchDis = refDto.BillRefVchDis,
                            BillRefVchTds = refDto.BillRefVchTds,
                            BillRefVchShort = refDto.BillRefVchShort,
                            BillRefVchBalance = refDto.BillRefVchBalance,
                            BillRefAccountId = refDto.BillRefAccountId
                        });
                    }
                }
            }
            if (journalDto.BillRefDetails != null && journalDto.BillRefDetails.Any())
            {
                foreach (var refDetail in journalDto.BillRefDetails)
                {
                    var bill = await _context.Bills
                        .FirstOrDefaultAsync(b =>
                            b.BillId == refDetail.BillRefVchId &&
                            b.BillStatus == true &&
                            b.BillCompanyId == GetCompanyId());

                    if (bill == null)
                        continue;

                    // 🔹 Already settled from DB (OLD receipts)
                    var dbSettled = await _context.BillRefDetails
      .Where(r =>
          r.BillRefVchId == bill.BillId &&
          r.BillRefAgainstId != journalDto.JournalId) // 👈 IMPORTANT
      .SumAsync(r =>
          r.BillRefVchAmount +
          r.BillRefVchDis +
          r.BillRefVchTds +
          r.BillRefVchShort);

                    // 🔹 Current receipt settlement (NOT yet in DB)
                    var currentSettlement =
                        refDetail.BillRefVchAmount +
                        refDetail.BillRefVchDis +
                        refDetail.BillRefVchTds +
                        refDetail.BillRefVchShort;

                    var totalSettled = dbSettled + currentSettlement;

                    bill.Bill_due_amt = Math.Round(
                        Math.Max(bill.BillNetAmount - totalSettled, 0),
                        2
                    );

                    bill.BillUpdated = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();

            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Journals",
                RecordId = journal.JournalId,
                VoucherType = journal.Voucher?.VoucherName,
                Amount = (int)journal.JournalTotal,
                Operations = "UPDATE",
                Remarks = journal.Voucher?.VoucherName + " Journal No: " + journal.JournalNoStr,
                YearId = journal.JournalYearId
            }, GetCompanyId());

            return NoContent();
        }

        // DELETE: api/Journals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJournal(int id)
        {
            var journal = await FilterByCompany(_context.Journals, "JournalCompanyId")
                .Include(j => j.BillRefDetails)
                .Include(j => j.Voucher)
                .FirstOrDefaultAsync(j => j.JournalId == id);

            if (journal == null)
                return NotFound();

            if (!string.IsNullOrEmpty(journal.JournalLockedBy))
            {
                return BadRequest(new { Message = "Journal is locked" });
            }

            // 🔥 HARD DELETE: BillRefDetails first
            if (journal.BillRefDetails != null && journal.BillRefDetails.Any())
            {
                _context.BillRefDetails.RemoveRange(journal.BillRefDetails);
            }
            // 🔹 Recalculate bill due BEFORE deleting refs
            if (journal.BillRefDetails != null && journal.BillRefDetails.Any())
            {
                foreach (var refDetail in journal.BillRefDetails)
                {
                    var bill = await _context.Bills
                        .FirstOrDefaultAsync(b =>
                            b.BillId == refDetail.BillRefVchId &&
                            b.BillStatus == true &&
                            b.BillCompanyId == GetCompanyId());

                    if (bill == null)
                        continue;

                    // 🔹 Total settled EXCLUDING this journal
                    var totalSettled = await _context.BillRefDetails
                        .Where(r =>
                            r.BillRefVchId == bill.BillId &&
                            r.BillRefAgainstId != journal.JournalId)
                        .SumAsync(r =>
                            r.BillRefVchAmount +
                            r.BillRefVchDis +
                            r.BillRefVchTds +
                            r.BillRefVchShort);

                    bill.Bill_due_amt = Math.Round(
                        Math.Max(bill.BillNetAmount - totalSettled, 0),
                        2
                    );

                    bill.BillUpdated = DateTime.UtcNow;
                }

                // 🔥 Delete BillRefDetails
                _context.BillRefDetails.RemoveRange(journal.BillRefDetails);
            }

           

            // 🔥 HARD DELETE: Journal
            _context.Journals.Remove(journal);
            await _context.SaveChangesAsync();

            // ✅ Audit log
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Journals",
                RecordId = id,
                VoucherType = journal.Voucher?.VoucherName,
                Amount = (int)journal.JournalTotal,
                Operations = "DELETE",
                Remarks = $"{journal.Voucher?.VoucherName} Journal No: {journal.JournalNoStr}",
                YearId = journal.JournalYearId
            }, GetCompanyId());

            return Ok( true);
        }

    }
}