using FreightBKShipping.Data;
using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Services
{
    public class CompanySubscriptionService : ICompanySubscriptionService
    {
        private readonly AppDbContext _context;

        public CompanySubscriptionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanySubscription>> GetAllAsync()
        {
            return await _context.CompanySubscriptions
                                 .Include(x => x.Company)
                                 .ToListAsync();
        }

        public async Task<CompanySubscription?> GetByIdAsync(int id)
        {
            return await _context.CompanySubscriptions
                                 .Include(x => x.Company)
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<CompanySubscription>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.CompanySubscriptions
                                 .Where(x => x.CompanyId == companyId)
                                 .ToListAsync();
        }

        public async Task<CompanySubscription> CreateAsync(CompanySubscription model)
        {
            model.CreatedAt = DateTime.UtcNow;
            model.IsActive = true;

            _context.CompanySubscriptions.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<bool> UpdateAsync(int id, CompanySubscription model)
        {
            var existing = await _context.CompanySubscriptions.FindAsync(id);
            if (existing == null)
                return false;

            existing.StartDate = model.StartDate;
            existing.EndDate = model.EndDate;
            existing.Days = model.Days;
            existing.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.CompanySubscriptions.FindAsync(id);
            if (existing == null)
                return false;

            _context.CompanySubscriptions.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
