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
    public class HsnSacController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;

        public HsnSacController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
            _context = context;
        }

        // GET: api/HsnSac
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HsnSac>>> GetHsnSacs()
        {
            return await FilterByCompany(_context.HsnSacs, "HsnCompanyId").OrderByDescending(b=>b.HsnId).ToListAsync();
        }

        // GET: api/HsnSac/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HsnSac>> GetHsnSac(int id)
        {
            var hsnSac = await FilterByCompany( _context.HsnSacs, "HsnCompanyId").FirstOrDefaultAsync(b => b.HsnId == id);

            if (hsnSac == null)
                return NotFound();

            return hsnSac;
        }

        // POST: api/HsnSac
        [HttpPost]
        public async Task<ActionResult<HsnSac>> PostHsnSac(HsnSac hsnSac)
        {
            var gstSlab = await _context.GstSlabs
        .FirstOrDefaultAsync(x => x.GstSlabId == hsnSac.HsnGstSlabId);

            if (gstSlab != null)
                hsnSac.HsnGstPer = (float)gstSlab.GstSlabIgstPer;
            hsnSac.HsnCreated = DateTime.UtcNow;
            hsnSac.HsnUpdated = DateTime.UtcNow;
            hsnSac.HsnAddedByUserId = GetUserId();
            hsnSac.HsnUpdatedByUserId = GetUserId();
            hsnSac.HsnCompanyId = GetCompanyId();
            _context.HsnSacs.Add(hsnSac);
            await _context.SaveChangesAsync();

            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "HSN/SAC",
                RecordId = hsnSac.HsnId,
                VoucherType = "HSN/SAC",
                Amount = 0,
                Operations = "INSERT",
                Remarks = hsnSac.HsnName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());

            return CreatedAtAction(nameof(GetHsnSac), new { id = hsnSac.HsnId }, hsnSac);
        }

        // PUT: api/HsnSac/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHsnSac(int id, HsnSac hsnSac)
        {
            if (id != hsnSac.HsnId)
                return BadRequest();
            // ✅ Recalculate GST percentage if slab changed
            var gstSlab = await _context.GstSlabs
                .FirstOrDefaultAsync(x => x.GstSlabId == hsnSac.HsnGstSlabId);
            if (gstSlab != null)
                hsnSac.HsnGstPer = (float)gstSlab.GstSlabIgstPer;
            hsnSac.HsnUpdated = DateTime.UtcNow;
            hsnSac.HsnUpdatedByUserId = GetUserId();
            hsnSac.HsnAddedByUserId = GetUserId();
            hsnSac.HsnCompanyId = GetCompanyId();
            _context.Entry(hsnSac).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.HsnSacs.Any(e => e.HsnId == id))
                    return NotFound();
                else
                    throw;
            }
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "HSN/SAC",
                RecordId = hsnSac.HsnId,
                VoucherType = "HSN/SAC",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = hsnSac.HsnName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(true);
        }

        // DELETE: api/HsnSac/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHsnSac(int id)
        {
            var hsnSac = await FilterByCompany(_context.HsnSacs, "HsnCompanyId").FirstOrDefaultAsync(b => b.HsnId == id);
            if (hsnSac == null)
                return NotFound();

            _context.HsnSacs.Remove(hsnSac);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "HSN/SAC",
                RecordId = id,
                VoucherType = "HSN/SAC",
                Amount = 0,
                Operations = "DELETE",
                Remarks = hsnSac.HsnName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(true);
        }
    }
}
