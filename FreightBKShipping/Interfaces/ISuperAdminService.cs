using FreightBKShipping.Models;

namespace FreightBKShipping.Interfaces
{
  

    public interface ISuperAdminService
    {
        Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync();
        Task<List<UserLoginSessionDto>> GetCompanyLogsAsync(int companyId);
        Task ForceLogoutCompanyAsync(int companyId);


        // ── Support Tickets ───────────────────────────────────
        Task<List<SupportTicketAdminDto>> GetAllTicketsAsync(string? companyFilter, string? statusFilter, string? priorityFilter);
        Task<SupportTicketAdminDto?> GetTicketDetailAsync(int ticketId);
        Task<TicketMessage> SendSupportReplyAsync(int ticketId, string message, string adminUserId);
        Task<bool> AssignTicketAsync(int ticketId, string assignedToUserId, string adminUserId);
        Task<bool> UpdateTicketStatusAsync(int ticketId, int statusId, int priorityId, string adminUserId);
        Task<bool> CloseTicketAsync(int ticketId, string adminUserId);
        Task<List<AdminUserDto>> GetSuperAdminUsersAsync();
    }

}
