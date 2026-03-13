using FreightBKShipping.DTOs;
using FreightBKShipping.Models;

namespace FreightBKShipping.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<SupportTicket>> GetTicketsAsync(int companyId);
        Task<SupportTicket?> GetTicketAsync(int ticketId, int companyId);
        Task<SupportTicket> CreateTicketAsync(TicketCreateDto dto, int companyId, string userId);
        Task<TicketMessage> SendReplyAsync(TicketReplyDto dto, string userId);
        Task<IEnumerable<TicketMessage>> GetRepliesAsync(int ticketId, int companyId);
        Task<bool> UpdateTicketAsync(int ticketId, TicketUpdateDto dto, int companyId, string userId);
        Task<bool> CloseTicketAsync(int ticketId, string userId);
    }
}
