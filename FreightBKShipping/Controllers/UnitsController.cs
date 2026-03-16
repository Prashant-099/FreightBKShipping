using FreightBKShipping.Data;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
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
        private readonly AuditLogService _auditLogService;

        public UnitsController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;

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
            var exists = await _context.Units.AnyAsync(u =>
    u.UnitCompanyId == GetCompanyId() &&
    u.UnitName.Trim().ToLower() == unit.UnitName.Trim().ToLower()
);

            if (exists)
            {
                return BadRequest(new { message = "Unit Name already exists." });
            }

            unit.UnitCompanyId = GetCompanyId();
            unit.UnitAddedByUserId = GetUserId();
            unit.UnitUpdatedByUserId = GetUserId();
            unit.UnitCreated = DateTime.UtcNow;
            unit.UnitUpdated = DateTime.UtcNow;
            unit.UnitStatus = true;

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Unit",
                RecordId = unit.UnitId,
                VoucherType = "Unit",
                Amount = 0,
                Operations = "INSERT",
                Remarks = $"{unit.UnitName}({unit.UnitFormalName})",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
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

            var exists = await _context.Units.AnyAsync(u =>
       u.UnitCompanyId == GetCompanyId() &&
       u.UnitId != id &&
       u.UnitName.Trim().ToLower() == unit.UnitName.Trim().ToLower()
   );

            if (exists)
            {
                return BadRequest(new { message = "Unit Name already exists." });
            }
            // update fields
            existing.UnitName = unit.UnitName;
            existing.UnitFormalName = unit.UnitFormalName;
            existing.UnitGstUnit = unit.UnitGstUnit;
            existing.UnitStatus = unit.UnitStatus;
            existing.UnitUpdatedByUserId = GetUserId();
            existing.UnitUpdated = DateTime.UtcNow;
            existing.UnitCompanyId = GetCompanyId();

            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Unit",
                RecordId = unit.UnitId,
                VoucherType = "Unit",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = $"{unit.UnitName}({unit.UnitFormalName})",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return NoContent();
        }

        // DELETE: api/Units/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            var query = FilterByCompany(_context.Units.AsQueryable(), "UnitCompanyId");
            var unit = await query.FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null) return NotFound();

            // 🔎 Check if used in ServiceTable
            bool usedInService = await _context.Services.AnyAsync(s =>
                s.ServiceUnitId == id &&
                s.ServiceCompanyId == GetCompanyId() &&
                s.ServiceStatus == true
            );

            if (usedInService)
            {
                return BadRequest(new
                {
                    message = $"THis Unit  is used in Service ."
                });
            }

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Unit",
                RecordId = id,
                VoucherType = "Unit",
                Amount = 0,
                Operations = "DELETE",
                Remarks = $"{unit.UnitName}({unit.UnitFormalName})",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(true);
        }
    }
}
