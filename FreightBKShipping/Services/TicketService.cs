using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.SignalR;
using FreightBKShipping.Models;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        // ✅ FIX 2: _hubContext removed from TicketService.
        //
        // Previously BOTH TicketsController AND TicketService were broadcasting
        // the same SignalR event, causing every message to fire twice.
        //
        // Rule: only the CONTROLLER layer should broadcast. The service layer
        // is responsible only for database persistence.
        // TicketsController.Reply() already calls _hub.Clients.Group(...).SendAsync()
        // after calling this service — so this service must NOT do it too.

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

        //public async Task<SupportTicket> CreateTicketAsync(TicketCreateDto dto, int companyId, string userId)
        //{
        //    var ticket = new SupportTicket
        //    {
        //        TicketNo = "TKT-" + DateTime.UtcNow.Ticks,
        //        Subject = dto.Subject,
        //        CompanyId = companyId,
        //        CreatedBy = userId,
        //        PriorityId = dto.PriorityId,
        //        StatusId = 1,
        //        CreatedAt = DateTime.UtcNow,
        //    };

        //    _context.SupportTickets.Add(ticket);
        //    await _context.SaveChangesAsync();

        //    var message = new TicketMessage
        //    {
        //        TicketId = ticket.TicketId,
        //        MessageText = dto.MessageText,
        //        SenderId = userId,
        //        SenderType = "User",
        //        IsReadByUser = true,
        //        IsReadBySupport = false,
        //    };

        //    _context.TicketMessages.Add(message);
        //    await _context.SaveChangesAsync();

        //    return ticket;
        //}
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

            var ticket = await _context.SupportTickets.FindAsync(dto.TicketId);
            if (ticket != null)
            {
                ticket.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            // ✅ FIX 2: NO SignalR broadcast here.
            // TicketsController.Reply() broadcasts after calling this method.
            // Doing it here too causes the event to fire twice per message.

            return message;
        }

        public async Task<IEnumerable<TicketMessage>> GetRepliesAsync(int ticketId, int companyId)
        {
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

            if (ticket == null) return false;

            ticket.StatusId = dto.StatusId;
            ticket.PriorityId = dto.PriorityId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CloseTicketAsync(int ticketId, string userId)
        {
            var ticket = await _context.SupportTickets.FindAsync(ticketId);
            if (ticket == null) return false;

            ticket.StatusId = 5;
            ticket.ClosedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // ✅ FIX 2: NO SignalR broadcast here.
            // TicketsController.Close() already broadcasts "TicketClosed" after
            // calling this method. Broadcasting here too fired it twice.

            return true;
        }



    }
}