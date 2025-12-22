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
    public class UnitsController : BaseController
    {
        private readonly AppDbContext _context;

        public UnitsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Units
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unit>>> GetUnits()
        {
            var query = FilterByCompany(_context.Units.AsQueryable(), "UnitCompanyId").OrderByDescending(b=>b.UnitId);
            return await query.ToListAsync();
        }

        // GET: api/Units/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Unit>> GetUnit(int id)
        {
            var query = FilterByCompany(_context.Units.AsQueryable(), "UnitCompanyId");
            var unit = await query.FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null) return NotFound();
            return unit;
        }

        // POST: api/Units
        [HttpPost]
        public async Task<ActionResult<Unit>> CreateUnit(Unit unit)
        {
            unit.UnitCompanyId = GetCompanyId();
            unit.UnitAddedByUserId = GetUserId();
            unit.UnitUpdatedByUserId = GetUserId();
            unit.UnitCreated = DateTime.UtcNow;
            unit.UnitUpdated = DateTime.UtcNow;
            unit.UnitStatus = true;

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUnit), new { id = unit.UnitId }, unit);
        }

        // PUT: api/Units/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, Unit unit)
        {
            if (id != unit.UnitId) return BadRequest();

            var query = FilterByCompany(_context.Units.AsQueryable(), "UnitCompanyId");
            var existing = await query.FirstOrDefaultAsync(u => u.UnitId == id);

            if (existing == null) return NotFound();

            // update fields
            existing.UnitName = unit.UnitName;
            existing.UnitFormalName = unit.UnitFormalName;
            existing.UnitGstUnit = unit.UnitGstUnit;
            existing.UnitStatus = unit.UnitStatus;
            existing.UnitUpdatedByUserId = GetUserId();
            existing.UnitUpdated = DateTime.UtcNow;
            existing.UnitCompanyId = GetCompanyId();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Units/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var query = FilterByCompany(_context.Units.AsQueryable(), "UnitCompanyId");
            var unit = await query.FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null) return NotFound();

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
