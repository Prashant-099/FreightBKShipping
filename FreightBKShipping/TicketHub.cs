using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FreightBKShipping.SignalR
{
    [Authorize]  // ✅ FIX 1: Hub pe auth enforce karo — bina token ke connect nahi hoga
    public class TicketHub : Hub
    {
        public async Task JoinTicket(int ticketId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        }

        public async Task LeaveTicket(int ticketId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        }
    }
}