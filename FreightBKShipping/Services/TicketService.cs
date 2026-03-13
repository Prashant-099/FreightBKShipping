using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupportTicket>> GetTicketsAsync(int companyId)
        {
            return await _context.SupportTickets
                .Where(t => t.CompanyId == companyId)
                .OrderByDescending(t => t.TicketId)
                .ToListAsync();
        }

        public async Task<SupportTicket?> GetTicketAsync(int ticketId, int companyId)
        {
            return await _context.SupportTickets
                .FirstOrDefaultAsync(t => t.TicketId == ticketId && t.CompanyId == companyId);
        }

        public async Task<SupportTicket> CreateTicketAsync(TicketCreateDto dto, int companyId, string userId)
        {
            var ticket = new SupportTicket
            {
                TicketNo = "TKT-" + DateTime.UtcNow.Ticks,
                Subject = dto.Subject,
                CompanyId = companyId,
                CreatedBy = userId,
                PriorityId = dto.PriorityId,
                StatusId = 1,
                CreatedAt = DateTime.UtcNow,
            };

            _context.SupportTickets.Add(ticket);
            await _context.SaveChangesAsync();

            var message = new TicketMessage
            {
                TicketId = ticket.TicketId,
                MessageText = dto.MessageText,
                SenderId = userId,
                SenderType = "User",
                IsReadByUser = true,
                IsReadBySupport = false,
            };

            _context.TicketMessages.Add(message);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<TicketMessage> SendReplyAsync(TicketReplyDto dto, string userId)
        {
            var message = new TicketMessage
            {
                TicketId = dto.TicketId,
                MessageText = dto.MessageText,
                SenderId = userId,
                SenderType = dto.SenderType,
                IsReadByUser = dto.SenderType == "User",
                IsReadBySupport = dto.SenderType == "Support",
            };

            _context.TicketMessages.Add(message);
            await _context.SaveChangesAsync();

            // Mark ticket as updated
            var ticket = await _context.SupportTickets.FindAsync(dto.TicketId);
            if (ticket != null)
            {
                ticket.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return message;
        }

        public async Task<IEnumerable<TicketMessage>> GetRepliesAsync(int ticketId, int companyId)
        {
            // Verify ticket belongs to company before returning messages
            var ticketExists = await _context.SupportTickets
                .AnyAsync(t => t.TicketId == ticketId && t.CompanyId == companyId);

            if (!ticketExists)
                return Enumerable.Empty<TicketMessage>();

            return await _context.TicketMessages
                .Where(m => m.TicketId == ticketId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateTicketAsync(int ticketId, TicketUpdateDto dto, int companyId, string userId)
        {
            var ticket = await _context.SupportTickets
                .FirstOrDefaultAsync(t => t.TicketId == ticketId && t.CompanyId == companyId);

            if (ticket == null)
                return false;

            ticket.StatusId = dto.StatusId;
            ticket.PriorityId = dto.PriorityId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseTicketAsync(int ticketId, string userId)
        {
            var ticket = await _context.SupportTickets.FindAsync(ticketId);

            if (ticket == null)
                return false;

            ticket.StatusId = 5;
            ticket.ClosedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
