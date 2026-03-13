using FreightBKShipping.Data;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GstSlabController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public GstSlabController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
            _context = context;
        }

        // GET: api/GstSlab
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GstSlab>>> GetGstSlabs()
        {
            return await FilterByCompany( _context.GstSlabs, "GstSlabCompanyId").OrderByDescending(b=>b.GstSlabId).ToListAsync();
        }

        // GET: api/GstSlab/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GstSlab>> GetGstSlab(int id)
        {
            var slab = await FilterByCompany( _context.GstSlabs, "GstSlabCompanyId").FirstOrDefaultAsync(b => b.GstSlabId == id);

            if (slab == null)
                return NotFound();

            return slab;
        }

        // POST: api/GstSlab
        [HttpPost]
        public async Task<ActionResult<GstSlab>> PostGstSlab(GstSlab slab)
        {
            var duplicate = await _context.GstSlabs.AnyAsync(g =>
    g.GstSlabCompanyId == GetCompanyId() &&
    g.GstSlabIgstPer == slab.GstSlabIgstPer
);

            if (duplicate)
            {
                return BadRequest(new { message = "GstSlab already exists." });
            }
            slab.GstSlabCreated = DateTime.UtcNow;
            slab.GstSlabUpdated = DateTime.UtcNow;
            slab.GstSlabUpdatedByUserId = GetUserId();
            slab.GstSlabAddedByUserId = GetUserId();
            slab.GstSlabCompanyId = GetCompanyId();
            _context.GstSlabs.Add(slab);
            await _context.SaveChangesAsync();

            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "GSTslab",
                RecordId = slab.GstSlabId,
                VoucherType = "GSTslab",
                Amount = 0,
                Operations = "INSERT",
                Remarks = slab.GstSlabName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());

            return CreatedAtAction(nameof(GetGstSlab), new { id = slab.GstSlabId }, slab);
        }

        // PUT: api/GstSlab/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGstSlab(int id, GstSlab slab)
        {
            if (id != slab.GstSlabId)
                return BadRequest();

            var duplicate = await _context.GstSlabs.AnyAsync(g =>
    g.GstSlabCompanyId == GetCompanyId() &&
    g.GstSlabIgstPer == slab.GstSlabIgstPer &&
    g.GstSlabId != id
);

            if (duplicate)
            {
                return BadRequest(new { message = "GstSlab already exists." });
            }

            slab.GstSlabUpdated = DateTime.UtcNow;
            slab.GstSlabUpdatedByUserId = GetUserId();
            slab.GstSlabCompanyId = GetCompanyId();
            _context.Entry(slab).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.GstSlabs.Any(e => e.GstSlabId == id))
                    return NotFound();
                else
                    throw;
            }
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "GSTslab",
                RecordId = slab.GstSlabId,
                VoucherType = "GSTslab",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = slab.GstSlabName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return NoContent();
        }

        // DELETE: api/GstSlab/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGstSlab(int id)
        {
            var slab = await FilterByCompany(_context.GstSlabs, "GstSlabCompanyId").FirstOrDefaultAsync(b => b.GstSlabId == id);
            if (slab == null)
                return NotFound();

            // 🔍 Check if GST Slab used in HSN/SAC
            bool usedInHsn = await _context.HsnSacs.AnyAsync(h =>
                h.HsnGstSlabId == id &&
                h.HsnCompanyId == GetCompanyId()
            );

            if (usedInHsn)
            {
                return BadRequest(new
                {
                    message = $"This GST Slab  is used in HSN/SAC."
                });
            }

            _context.GstSlabs.Remove(slab);
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "GSTslab",
                RecordId = id,
                VoucherType = "GSTslab",
                Amount = 0,
                Operations = "DELETE",
                Remarks = slab.GstSlabName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
