using FreightBKShipping.Models;

namespace FreightBKShipping.Interfaces
{
  

    public interface ISuperAdminService
    {
        Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync();
        Task<List<UserLoginSessionDto>> GetCompanyLogsAsync(int companyId);
        Task ForceLogoutCompanyAsync(int companyId);
    }

}
