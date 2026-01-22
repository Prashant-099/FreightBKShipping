using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatesController : BaseController
    {
        private readonly AppDbContext _context;

        public StatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            // ❌ Company filter removed
            // var query = FilterByCompany(_context.States.AsQueryable(), "StateCompanyId");

            var states = await _context.States.ToListAsync();
            return Ok(states);
        }

        // GET: api/States/GetByCode/GJ
        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<State>> GetStateByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("State code is required");

            // ❌ Company filter removed
            // var query = FilterByCompany(_context.States.AsQueryable(), "StateCompanyId");

            var state = await _context.States
                .FirstOrDefaultAsync(s => s.StateCode.ToLower() == code.Trim().ToLower());

            if (state == null)
                return NotFound($"State code '{code}' not found");

            return Ok(state);
        }

        // GET: api/States/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
            // ❌ Company filter removed
            // var query = FilterByCompany(_context.States.AsQueryable(), "StateCompanyId");

            var state = await _context.States
                                      .FirstOrDefaultAsync(s => s.StateId == id);

            if (state == null)
                return NotFound();

            return Ok(state);
        }

        // POST: api/States
        [HttpPost]
        public async Task<ActionResult<State>> CreateState(State state)
        {
            // 🔹 Ye rehne diya, future multi-company ke kaam aa sakta hai
            state.StateCompanyId = GetCompanyId();
            state.StateAddedByUserId = GetUserId();
            state.StateCreated = DateTime.UtcNow;
            state.StateUpdated = DateTime.UtcNow;
            state.StateStatus = true;

            _context.States.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetState), new { id = state.StateId }, state);
        }

        // PUT: api/States/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(int id, State state)
        {
            if (id != state.StateId)
                return BadRequest();

            // ❌ Company filter removed
            // var query = FilterByCompany(_context.States.AsQueryable(), "StateCompanyId");
            // var existing = await query.FirstOrDefaultAsync(s => s.StateId == id);

            var existing = await _context.States
                                         .FirstOrDefaultAsync(s => s.StateId == id);

            if (existing == null)
                return NotFound();

            existing.StateName = state.StateName;
            existing.StateCode = state.StateCode;
            existing.StateStatus = state.StateStatus;
            existing.StateUpdatedByUserId = GetUserId();
            existing.StateCompanyId = GetCompanyId(); // optional
            existing.StateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/States/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            // ❌ Company filter removed
            // var query = FilterByCompany(_context.States.AsQueryable(), "StateCompanyId");
            // var state = await query.FirstOrDefaultAsync(s => s.StateId == id);

            var state = await _context.States
                                      .FirstOrDefaultAsync(s => s.StateId == id);

            if (state == null)
                return NotFound();

            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
