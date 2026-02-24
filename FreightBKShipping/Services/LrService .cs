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
                throw new Exception("Error saving LR: " + ex.Message);
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
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
    }
}
