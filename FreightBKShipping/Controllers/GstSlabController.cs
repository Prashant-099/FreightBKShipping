using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GstSlabController : BaseController
    {
        private readonly AppDbContext _context;

        public GstSlabController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GstSlab
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GstSlab>>> GetGstSlabs()
        {
            return await FilterByCompany( _context.GstSlabs, "GstSlabCompanyId").ToListAsync();
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
            slab.GstSlabCreated = DateTime.UtcNow;
            slab.GstSlabUpdated = DateTime.UtcNow;
            slab.GstSlabUpdatedByUserId = GetUserId();
            slab.GstSlabAddedByUserId = GetUserId();
            slab.GstSlabCompanyId = GetCompanyId();
            _context.GstSlabs.Add(slab);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGstSlab), new { id = slab.GstSlabId }, slab);
        }

        // PUT: api/GstSlab/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGstSlab(int id, GstSlab slab)
        {
            if (id != slab.GstSlabId)
                return BadRequest();

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

            return NoContent();
        }

        // DELETE: api/GstSlab/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGstSlab(int id)
        {
            var slab = await FilterByCompany(_context.GstSlabs, "GstSlabCompanyId").FirstOrDefaultAsync(b => b.GstSlabId == id);
            if (slab == null)
                return NotFound();

            _context.GstSlabs.Remove(slab);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
