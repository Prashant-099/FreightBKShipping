using FreightBKShipping.DTOs.Dashboarddto;
using FreightBKShipping.Models;

namespace FreightBKShipping.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetDashboardAsync(
            int companyId,
            int yearId,
            DateTime? fromDate,
            DateTime? toDate);
    }
}
