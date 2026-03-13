using FreightBKShipping.DTOs;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : BaseController
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET api/Tickets
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _ticketService.GetTicketsAsync(GetCompanyId());
            return Ok(tickets);
        }

        // GET api/Tickets/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicketAsync(id, GetCompanyId());
            if (ticket == null)
                return NotFound(new { message = $"Ticket {id} not found." });
            return Ok(ticket);
        }

        // POST api/Tickets
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ticket = await _ticketService.CreateTicketAsync(dto, GetCompanyId(), GetUserId());
            return Ok(ticket);
        }

        // POST api/Tickets/reply
        [HttpPost("reply")]
        public async Task<IActionResult> Reply([FromBody] TicketReplyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reply = await _ticketService.SendReplyAsync(dto, GetUserId());
            return Ok(reply);
        }

        // GET api/Tickets/{id}/replies
        [HttpGet("{id}/replies")]
        public async Task<IActionResult> GetReplies(int id)
        {
            var replies = await _ticketService.GetRepliesAsync(id, GetCompanyId());
            return Ok(replies);
        }

        // PUT api/Tickets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] TicketUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ticketService.UpdateTicketAsync(id, dto, GetCompanyId(), GetUserId());
            if (!result)
                return NotFound(new { message = $"Ticket {id} not found or update failed." });
            return Ok(result);
        }

        // POST api/Tickets/close/{id}
        [HttpPost("close/{id}")]
        public async Task<IActionResult> Close(int id)
        {
            var result = await _ticketService.CloseTicketAsync(id, GetUserId());
            return Ok(result);
        }
    }
}
