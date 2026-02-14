using FreightBKShipping.Models;

namespace FreightBKShipping.Interfaces
{
    public interface ICompanySubscriptionService
    {
        Task<IEnumerable<CompanySubscription>> GetAllAsync();
        Task<CompanySubscription?> GetByIdAsync(int id);
        Task<IEnumerable<CompanySubscription>> GetByCompanyIdAsync(int companyId);
        Task<CompanySubscription> CreateAsync(CompanySubscription model);
        Task<bool> UpdateAsync(int id, CompanySubscription model);
        Task<bool> DeleteAsync(int id);
    }
}
