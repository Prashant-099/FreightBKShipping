using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreightBKShipping.Controllers
{
    
        [ApiController]
        [Route("api/superadmin")]
        [Authorize(Roles = "SuperAdmin")]
        public class SuperAdminController : BaseController
        {
            private readonly ISuperAdminService _service;

            public SuperAdminController(ISuperAdminService service)
            {
                _service = service;
            }

            [HttpGet("dashboard")]
            public async Task<IActionResult> GetDashboard()
            {
                return Ok(await _service.GetSuperAdminDashboardAsync());
            }

            [HttpGet("company-logs/{companyId}")]
            public async Task<IActionResult> GetCompanyLogs(int companyId)
            {
                return Ok(await _service.GetCompanyLogsAsync(companyId));
            }

            [HttpPost("force-logout/{companyId}")]
            public async Task<IActionResult> ForceLogout(int companyId)
            {
                await _service.ForceLogoutCompanyAsync(companyId);
                return Ok("Company logged out successfully.");
            }


        // ── Support Tickets ───────────────────────────────────────────

        // GET api/superadmin/tickets?company=1&status=1&priority=2
        [HttpGet("tickets")]
        public async Task<IActionResult> GetAllTickets(
            [FromQuery] string? company,
            [FromQuery] string? status,
            [FromQuery] string? priority)
        {
            var tickets = await _service.GetAllTicketsAsync(company, status, priority);
            return Ok(tickets);
        }

        // GET api/superadmin/tickets/{id}
        [HttpGet("tickets/{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticket = await _service.GetTicketDetailAsync(id);
            if (ticket == null) return NotFound(new { message = $"Ticket {id} not found." });
            return Ok(ticket);
        }

        // POST api/superadmin/tickets/{id}/reply
        [HttpPost("tickets/{id}/reply")]
        public async Task<IActionResult> Reply(int id, [FromBody] TicketReplyAdminDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest(new { message = "Message cannot be empty." });

            var msg = await _service.SendSupportReplyAsync(id, dto.Message, GetUserId());
            return Ok(msg);
        }

        // POST api/superadmin/tickets/{id}/assign
        [HttpPost("tickets/{id}/assign")]
        public async Task<IActionResult> Assign(int id, [FromBody] TicketAssignDto dto)
        {
            var result = await _service.AssignTicketAsync(id, dto.AssignedToUserId, GetUserId());
            if (!result) return NotFound(new { message = $"Ticket {id} not found." });
            return Ok(result);
        }

        // PUT api/superadmin/tickets/{id}/status
        [HttpPut("tickets/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketStatusUpdateDto dto)
        {
            var result = await _service.UpdateTicketStatusAsync(id, dto.StatusId, dto.PriorityId, GetUserId());
            if (!result) return NotFound(new { message = $"Ticket {id} not found." });
            return Ok(result);
        }

        // POST api/superadmin/tickets/{id}/close
        [HttpPost("tickets/{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
            var result = await _service.CloseTicketAsync(id, GetUserId());
            if (!result) return NotFound(new { message = $"Ticket {id} not found." });
            return Ok(result);
        }

        // GET api/superadmin/admin-users
        [HttpGet("admin-users")]
        public async Task<IActionResult> GetAdminUsers()
            => Ok(await _service.GetSuperAdminUsersAsync());
    }

    
}
