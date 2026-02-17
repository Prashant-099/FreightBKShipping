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
    }
}
