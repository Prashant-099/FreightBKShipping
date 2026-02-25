using FreightBKShipping.Data;
using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using Microsoft.EntityFrameworkCore;
using System;


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
        //public async Task<Lr> Create(Lr model)
        //{
        //    _context.Lrs.Add(model);

        //    await _context.SaveChangesAsync();
        //    return model;
        //}

        //public async Task<bool> Update(Lr model)
        //{
        //    var existing = await _context.Lrs.FindAsync(model.LrId);
        //    if (existing == null)
        //        return false;

        //    _context.Entry(existing).CurrentValues.SetValues(model);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}
        public async Task<Lr> Create(Lr model, List<LRDetail>? details, List<LRJournal>? journals)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
              

                // 🔥 Calculate Before Saving
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
                throw new Exception(
           ex.InnerException?.Message ?? ex.Message,
           ex);
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

                // 🔥 Calculate on existing object
                CalculateLrTotals(existing, details, journals);

                // Remove old child data
                var oldDetails = _context.LRDetails.Where(x => x.LrDetailsLrId == model.LrId);
                var oldJournals = _context.LRJournals.Where(x => x.LrId == model.LrId);

                _context.LRDetails.RemoveRange(oldDetails);
                _context.LRJournals.RemoveRange(oldJournals);

                await _context.SaveChangesAsync();

                // Insert new child data
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
                throw new Exception(
                    ex.InnerException?.Message ?? ex.Message,
                    ex);
            }
        }

        private void CalculateLrTotals(Lr model, List<LRDetail>? details, List<LRJournal>? journals)
        {
            // ================= KM Calculation =================
            double startKm = 0;
            double endKm = 0;

            double.TryParse(model.LrStartKm, out startKm);
            double.TryParse(model.LrEndKm, out endKm);

            double totalKm = endKm - startKm;
            if (totalKm < 0) totalKm = 0;

            model.LrTotKm = totalKm.ToString();

            // ================= Weight Calculation =================
            if (model.LrGrossWt.HasValue && model.LrTareWt.HasValue)
                model.LrLoadWt = model.LrGrossWt.Value - model.LrTareWt.Value;

            model.LrShortWt = model.LrLoadWt - model.LrUnloadWt;

            // ================= Bill Side =================
            // Bill Type logic
            switch (model.LrBillTypeBill)
            {
                case "Fixed Rate":
                    model.LrGrossFreightBill = model.LrRateBill;
                    break;

                case "Rate X Weight":
                    model.LrGrossFreightBill = model.LrRateBill * model.LrLoadWt;
                    break;

                case "Rate X KM":
                    model.LrGrossFreightBill = model.LrRateBill * totalKm;
                    break;

                default:
                    model.LrGrossFreightBill = 0;
                    break;
            }

            // Short Amount Bill
            model.LrShortAmtBill = model.LrShortRateBill * model.LrShortWt;

            // Detention Bill
            model.LrDetentionAmtBill = model.LrDetentionRateBill * model.LrDetentionDayBill;

            // GST Bill
            model.LrGstAmount = (float)((model.LrGrossFreightBill * model.LrGstPercentage) / 100);

            // Net Freight Bill
            model.LrNetFreightBill =
                model.LrGrossFreightBill
                + model.LrTripChargeBill
                + model.LrDetentionAmtBill
                + model.LrOtherChargeBill
                - model.LrAdvanceBill
                - model.LrShortAmtBill;

            // ================= Truck Side =================
            model.LrGrossFreightTruck = model.LrBillRateTruck * model.LrLoadWt;

            model.LrShortAmtTruck = model.LrShortRateTruck * model.LrShortWt;

            model.LrDetentionAmtTruck = model.LrDetentionRateTruck * model.LrDetentionDayTruck;

            model.LrNetFreightTruck =
                model.LrGrossFreightTruck
                + model.LrTripChargeTruck
                + model.LrDetentionAmtTruck
                - model.LrTripAdvance
                - model.LrShortAmtTruck;

            // ================= Journal Total =================
            if (journals != null && journals.Any())
                model.LrJournalAmt = (float)journals.Sum(x => x.Amount);
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

        public async Task<List<LrListVM>> GetAllForList()
        {
            var query =
                from lr in _context.Lrs.AsNoTracking()

                    // Party
                join party in _context.Accounts
                    on lr.LrPartyAccountId equals party.AccountId into partyJoin
                from party in partyJoin.DefaultIfEmpty()

                    // Supplier
                join supplier in _context.Accounts
                    on lr.LrSupplierAccountId equals supplier.AccountId into supplierJoin
                from supplier in supplierJoin.DefaultIfEmpty()

                    // Driver
                join driver in _context.Accounts
                    on lr.LrDriverId equals driver.AccountId into driverJoin
                from driver in driverJoin.DefaultIfEmpty()

                    // From Location
                join fromLoc in _context.Locations
                    on lr.LrFromLocationId equals fromLoc.LocationId into fromJoin
                from fromLoc in fromJoin.DefaultIfEmpty()

                    // To Location
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

                    PartyName = party != null ? party.AccountName : null,
                    SupplierName = supplier != null ? supplier.AccountName : null,
                    DriverName = driver != null ? driver.AccountName : null,

                    //VehicleNo = lr.,

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

                    LrStatus = lr.LrStatus
                };

            return await query.ToListAsync();
        }
    }


   
    }
