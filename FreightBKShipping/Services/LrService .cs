using FreightBKShipping.Data;
using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FreightBKShipping.Services
{
    public class LrService : ILrService
    {
        private readonly AppDbContext _context;

        public LrService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Lr>> SearchByPartyAndDate(
            int partyId,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var query =
                from l in _context.Lrs
                join a in _context.Accounts
                    on l.LrPartyAccountId equals a.AccountId
                where a.AccountId == partyId
                      && l.LrStatus != 2
                      && (!fromDate.HasValue || l.LrDate >= fromDate.Value)
                      && (!toDate.HasValue || l.LrDate <= toDate.Value.Date.AddDays(1).AddTicks(-1))
                select l;

            return await query
                .OrderByDescending(l => l.LrDate)
                .ToListAsync();
        }

        public async Task<List<Lr>> GetAll()
        {
            return await _context.Lrs
                .Where(x => x.LrStatus != 2)
                .OrderByDescending(x => x.LrId)
                .ToListAsync();
        }

        public async Task<Lr?> GetById(int id)
        {
            return await _context.Lrs
                .FirstOrDefaultAsync(x => x.LrId == id && x.LrStatus != 2);
        }

        public async Task<LrEntryDto?> GetEntryById(int id)
        {
            var lr = await _context.Lrs
                .FirstOrDefaultAsync(x => x.LrId == id && x.LrStatus != 2);

            if (lr == null) return null;

            var details = await _context.LRDetails
                .Where(x => x.LrDetailsLrId == id)
                .ToListAsync();

            var journals = await _context.LRJournals
                .Where(x => x.LrId == id)
                .ToListAsync();

            return new LrEntryDto
            {
                Main = lr,
                Details = details,
                Journals = journals
            };
        }

        public async Task<Lr> Create(Lr model, List<LRDetail>? details, List<LRJournal>? journals)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                CalculateLrTotals(model, details, journals);

                _context.Lrs.Add(model);
                await _context.SaveChangesAsync();

                int lrId = model.LrId;

                if (details != null && details.Any())
                {
                    foreach (var d in details)
                        d.LrDetailsLrId = lrId;

                    await _context.LRDetails.AddRangeAsync(details);
                }

                if (journals != null && journals.Any())
                {
                    foreach (var j in journals)
                    {
                        j.LrId = lrId;
                        j.Created = DateTime.UtcNow;
                        j.Updated = DateTime.UtcNow;
                    }

                    await _context.LRJournals.AddRangeAsync(journals);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return model;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        public async Task<bool> Update(Lr model, List<LRDetail> details, List<LRJournal> journals)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existing = await _context.Lrs.FindAsync(model.LrId);
                if (existing == null)
                    return false;

                _context.Entry(existing).CurrentValues.SetValues(model);

                CalculateLrTotals(existing, details, journals);

                var oldDetails = _context.LRDetails.Where(x => x.LrDetailsLrId == model.LrId);
                var oldJournals = _context.LRJournals.Where(x => x.LrId == model.LrId);

                _context.LRDetails.RemoveRange(oldDetails);
                _context.LRJournals.RemoveRange(oldJournals);

                await _context.SaveChangesAsync();

                foreach (var d in details)
                    d.LrDetailsLrId = model.LrId;

                foreach (var j in journals)
                {
                    j.LrId = model.LrId;
                    j.Updated = DateTime.UtcNow;
                }

                await _context.LRDetails.AddRangeAsync(details);
                await _context.LRJournals.AddRangeAsync(journals);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.InnerException?.Message ?? ex.Message, ex);
            }
        }

        private void CalculateLrTotals(Lr model, List<LRDetail>? details, List<LRJournal>? journals)
        {
            // ── KM ────────────────────────────────────────────────────
            double.TryParse(model.LrStartKm, out double startKm);
            double.TryParse(model.LrEndKm, out double endKm);
            double totalKm = Math.Max(0, endKm - startKm);
            model.LrTotKm = totalKm.ToString();

            // ── Weight ────────────────────────────────────────────────
            if (model.LrGrossWt.HasValue && model.LrTareWt.HasValue)
                model.LrLoadWt = model.LrGrossWt.Value - model.LrTareWt.Value;

            model.LrShortWt = model.LrLoadWt - model.LrUnloadWt;

            var filledDetails = details?
                .Where(d => (d.LrDetailProductId ?? 0) > 0 || (d.LrDetailGrossFreight ?? 0) > 0)
                .ToList();

            if (filledDetails != null && filledDetails.Count > 0)
            {
                model.LrGrossFreightBill = filledDetails
                    .Sum(d => (double)(d.LrDetailGrossFreight ?? 0));
            }

            // ── Bill Side ─────────────────────────────────────────────
            model.LrShortAmtBill = model.LrShortRateBill * model.LrShortWt;
            model.LrDetentionAmtBill = model.LrDetentionRateBill * model.LrDetentionDayBill;
            model.LrGstAmount = (float)((model.LrGrossFreightBill * model.LrGstPercentage) / 100);

            model.LrNetFreightBill =
                model.LrGrossFreightBill
                + model.LrTripChargeBill
                + model.LrDetentionAmtBill
                + model.LrOtherChargeBill
                - model.LrAdvanceBill
                - model.LrShortAmtBill;

            // ── Truck Side ────────────────────────────────────────────
            model.LrGrossFreightTruck = model.LrBillRateTruck * model.LrLoadWt;
            model.LrShortAmtTruck = model.LrShortRateTruck * model.LrShortWt;
            model.LrDetentionAmtTruck = model.LrDetentionRateTruck * model.LrDetentionDayTruck;

            model.LrNetFreightTruck =
                model.LrGrossFreightTruck
                + model.LrTripChargeTruck
                + model.LrDetentionAmtTruck
                - model.LrTripAdvance
                - model.LrShortAmtTruck;

            // ── Journal Group Totals ──────────────────────────────────
            if (journals != null && journals.Any())
            {
                model.LrAdvanceTotal = journals
                    .Where(j => j.JournalGroup == LrJournalGroup.Advance)
                    .Sum(j => (double)j.Amount);

                model.LrDieselTotal = journals
                    .Where(j => j.JournalGroup == LrJournalGroup.Diesel)
                    .Sum(j => j.UreaAmount ?? 0);

                model.LrChargesTotal = journals
                    .Where(j => j.JournalGroup == LrJournalGroup.Charges)
                    .Sum(j => (double)j.Amount);

                model.LrExpenseTotal = journals
                    .Where(j => j.JournalGroup == LrJournalGroup.Expense)
                    .Sum(j => (double)j.Amount);

                model.LrAdvRecTotal = journals
                    .Where(j => j.JournalGroup == LrJournalGroup.AdvanceReceived)
                    .Sum(j => (double)j.Amount);

                model.LrJournalAmt = (float)journals.Sum(x => x.Amount);
            }
            else
            {
                model.LrAdvanceTotal = 0;
                model.LrDieselTotal = 0;
                model.LrChargesTotal = 0;
                model.LrExpenseTotal = 0;
                model.LrAdvRecTotal = 0;
                model.LrJournalAmt = 0;
            }

            // ── Net Freight Calc ──────────────────────────────────────
            model.LrNetFreightCalc =
                model.LrGrossFreightBill
                + model.LrChargesTotal
                - model.LrAdvanceTotal
                - model.LrDieselTotal
                - model.LrExpenseTotal;
        }

        public async Task<bool> Delete(int id)
        {
            var lr = await _context.Lrs.FindAsync(id);
            if (lr == null)
                return false;

            _context.Lrs.Remove(lr);
            await _context.SaveChangesAsync();

            return true;
        }

        // ═══════════════════════════════════════════════════════════════════
        // GetAllForList — AccountName fix: pehle AccountId se join,
        // phir Remarks fallback, phir "—"
        // ═══════════════════════════════════════════════════════════════════
        public async Task<List<LrListVM>> GetAllForList()
        {
            // ── Step 1: Main LR rows ─────────────────────────────────────
            var lrRows = await (
                from lr in _context.Lrs.AsNoTracking()

                join party in _context.Accounts
                    on lr.LrPartyAccountId equals party.AccountId into partyJoin
                from party in partyJoin.DefaultIfEmpty()

                join supplier in _context.Accounts
                    on lr.LrSupplierAccountId equals supplier.AccountId into supplierJoin
                from supplier in supplierJoin.DefaultIfEmpty()

                join driver in _context.Accounts
                    on lr.LrDriverId equals driver.AccountId into driverJoin
                from driver in driverJoin.DefaultIfEmpty()

                join vehicle in _context.Vehicles
                    on lr.LrVehicleId equals vehicle.VehicleId into vehicleJoin
                from vehicle in vehicleJoin.DefaultIfEmpty()

                join fromLoc in _context.Locations
                    on lr.LrFromLocationId equals fromLoc.LocationId into fromJoin
                from fromLoc in fromJoin.DefaultIfEmpty()

                join toLoc in _context.Locations
                    on lr.LrToLocationId equals toLoc.LocationId into toJoin
                from toLoc in toJoin.DefaultIfEmpty()

                where lr.LrStatus != 2
                orderby lr.LrId descending

                select new LrListVM
                {
                    LrId = lr.LrId,
                    LrNoStr = lr.LrNoStr,
                    LrDate = lr.LrDate,
                    LrTripNo = lr.LrTripNo.ToString(),
                    LrVehicleId = lr.LrVehicleId,

                    PartyName = party != null ? party.AccountName : null,
                    SupplierName = supplier != null ? supplier.AccountName : null,
                    DriverName = driver != null ? driver.AccountName : null,
                    VehicleNo = vehicle != null ? vehicle.VehicleNo : lr.LrNtVehicleNo,

                    FromLocationName = fromLoc != null ? fromLoc.LocationName : null,
                    ToLocationName = toLoc != null ? toLoc.LocationName : null,

                    LrLoadWt = lr.LrLoadWt,
                    LrUnloadWt = lr.LrUnloadWt,
                    LrShortWt = lr.LrShortWt,

                    LrBillTypeBill = lr.LrBillTypeBill,
                    LrRateBill = lr.LrRateBill,
                    LrGrossFreightBill = lr.LrGrossFreightBill,
                    LrNetFreightBill = lr.LrNetFreightBill,

                    LrBillRateTruck = lr.LrBillRateTruck,
                    LrNetFreightTruck = lr.LrNetFreightTruck,

                    LrGstPercentage = lr.LrGstPercentage,
                    LrGstAmount = lr.LrGstAmount,

                    LrAdvanceTotal = lr.LrAdvanceTotal,
                    LrDieselTotal = lr.LrDieselTotal,
                    LrChargesTotal = lr.LrChargesTotal,
                    LrExpenseTotal = lr.LrExpenseTotal,
                    LrAdvRecTotal = lr.LrAdvRecTotal,
                    LrNetFreightCalc = lr.LrNetFreightCalc,

                    LrStatus = lr.LrStatus,
                }
            ).ToListAsync();

            if (!lrRows.Any()) return lrRows;

            var lrIds = lrRows.Select(x => x.LrId).ToList();

            // ── Step 2: Product details ──────────────────────────────────
            var productRows = await _context.LRDetails.AsNoTracking()
                .Where(d => lrIds.Contains(d.LrDetailsLrId)
                         && ((d.LrDetailProductId ?? 0) > 0 || (d.LrDetailGrossFreight ?? 0) > 0))
                .Select(d => new
                {
                    LrId = d.LrDetailsLrId,
                    ProductName = d.LrDetailCargoName ?? "—",
                    BillType = d.LrDetailBillType ?? "—",
                    Rate = (double)(d.LrDetailBillRate ?? 0),
                    GrossFreight = (double)(d.LrDetailGrossFreight ?? 0),
                })
                .ToListAsync();

            // ── Step 3: Journal details ──────────────────────────────────
            //
            // ✅ FIX: Diesel journal mein AccountId NULL hota hai.
            //         Pump ka AccountId, PumpBillId column mein store hota hai.
            //         Isliye do alag LEFT JOIN lagate hain:
            //           - normalAcc  → AccountId   se (Advance/Charges/Expense/AdvRec)
            //           - pumpAcc    → PumpBillId   se (Diesel)
            //         Priority: normalAcc → pumpAcc → Remarks → "—"
            //
            var journalRows = await (
                from j in _context.LRJournals.AsNoTracking()

                    // JOIN 1: Normal accounts (Advance, Charges, Expense, AdvRec)
                join normalAcc in _context.Accounts
                    on j.AccountId equals normalAcc.AccountId into normalJoin
                from normalAcc in normalJoin.DefaultIfEmpty()

                    // JOIN 2: Pump account (Diesel — PumpBillId mein AccountId stored hai)
                join pumpAcc in _context.Accounts
                    on j.PumpBillId equals pumpAcc.AccountId into pumpJoin
                from pumpAcc in pumpJoin.DefaultIfEmpty()

                where j.LrId != null && lrIds.Contains(j.LrId.Value)

                select new
                {
                    LrId = j.LrId.Value,
                    j.JournalGroup,

                    // ✅ AccountName priority:
                    // 1. normalAcc  — Advance/Charges/Expense/AdvRec ke liye
                    // 2. pumpAcc    — Diesel ke liye (PumpBillId → AccountId)
                    // 3. Remarks    — fallback
                    // 4. "—"        — last resort
                    AccountName =
                        (normalAcc != null && normalAcc.AccountName != null && normalAcc.AccountName != "")
                            ? normalAcc.AccountName
                        : (pumpAcc != null && pumpAcc.AccountName != null && pumpAcc.AccountName != "")
                            ? pumpAcc.AccountName
                        : (j.Remarks != null && j.Remarks != "")
                            ? j.Remarks
                        : "—",

                    Amount = (double)(j.Amount ?? 0),
                    UreaAmount = j.UreaAmount ?? 0,
                    Remarks = j.Remarks,
                }
            ).ToListAsync();

            // ── Step 4: Map karo ─────────────────────────────────────────
            var productMap = productRows
                .GroupBy(x => x.LrId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var journalMap = journalRows
                .GroupBy(x => x.LrId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var lr in lrRows)
            {
                // Products
                if (productMap.TryGetValue(lr.LrId, out var prods))
                {
                    lr.Products = prods.Select(p => new LrListProductVM
                    {
                        ProductName = p.ProductName,
                        BillType = p.BillType,
                        Rate = p.Rate,
                        GrossFreight = p.GrossFreight,
                    }).ToList();
                }

                // Journals
                if (journalMap.TryGetValue(lr.LrId, out var jrnls))
                {
                    lr.AdvanceRows = jrnls
                        .Where(j => j.JournalGroup == LrJournalGroup.Advance && j.Amount > 0)
                        .Select(j => new LrListJournalVM
                        {
                            AccountName = j.AccountName,
                            Amount = j.Amount,
                            Remarks = j.Remarks,
                        }).ToList();

                    // Diesel: UreaAmount = diesel amount, AccountName = pump name
                    lr.DieselRows = jrnls
                        .Where(j => j.JournalGroup == LrJournalGroup.Diesel && j.UreaAmount > 0)
                        .Select(j => new LrListJournalVM
                        {
                            AccountName = j.AccountName,
                            Amount = j.UreaAmount,
                            Remarks = j.Remarks,
                        }).ToList();

                    lr.ChargesRows = jrnls
                        .Where(j => j.JournalGroup == LrJournalGroup.Charges && j.Amount > 0)
                        .Select(j => new LrListJournalVM
                        {
                            AccountName = j.AccountName,
                            Amount = j.Amount,
                            Remarks = j.Remarks,
                        }).ToList();

                    lr.ExpenseRows = jrnls
                        .Where(j => j.JournalGroup == LrJournalGroup.Expense && j.Amount > 0)
                        .Select(j => new LrListJournalVM
                        {
                            AccountName = j.AccountName,
                            Amount = j.Amount,
                            Remarks = j.Remarks,
                        }).ToList();

                    lr.AdvRecRows = jrnls
                        .Where(j => j.JournalGroup == LrJournalGroup.AdvanceReceived && j.Amount > 0)
                        .Select(j => new LrListJournalVM
                        {
                            AccountName = j.AccountName,
                            Amount = j.Amount,
                            Remarks = j.Remarks,
                        }).ToList();
                }
            }

            return lrRows;
        }
    }
}