using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HsnSacController : BaseController
    {
        private readonly AppDbContext _context;

        public HsnSacController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/HsnSac
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HsnSac>>> GetHsnSacs()
        {
            return await FilterByCompany(_context.HsnSacs, "HsnCompanyId").ToListAsync();
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
            hsnSac.HsnCreated = DateTime.UtcNow;
            hsnSac.HsnUpdated = DateTime.UtcNow;
            hsnSac.HsnAddedByUserId = GetUserId();
            hsnSac.HsnUpdatedByUserId = GetUserId();
            hsnSac.HsnCompanyId = GetCompanyId();
            _context.HsnSacs.Add(hsnSac);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHsnSac), new { id = hsnSac.HsnId }, hsnSac);
        }

        // PUT: api/HsnSac/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHsnSac(int id, HsnSac hsnSac)
        {
            if (id != hsnSac.HsnId)
                return BadRequest();

            hsnSac.HsnUpdated = DateTime.UtcNow;
            hsnSac.HsnUpdatedByUserId = GetUserId();
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

            return NoContent();
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

            return NoContent();
        }
    }
}
