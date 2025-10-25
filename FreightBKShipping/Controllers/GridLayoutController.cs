using FreightBKShipping.Controllers;
using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GridLayoutController : BaseController
    {
        private readonly AppDbContext _context;

        public GridLayoutController(AppDbContext context) => _context = context;

        // GET: api/GridLayout
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId");
            return Ok(await query.ToListAsync());
        }

        // GET: api/GridLayout/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var layout = await FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                .FirstOrDefaultAsync(g => g.GridLayoutId == id);

            if (layout == null) return NotFound();
            return Ok(layout);
        }

        // GET: api/GridLayout/ByVoucherType/ACCOUNT
        [HttpGet("ByVoucherType/{voucherType}")]
        public async Task<IActionResult> GetByVoucherType(string voucherType)
        {
            var query = FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                .Where(g => g.GridLayoutVoucherType == voucherType)
                .OrderByDescending(g => g.GridLayoutDefault)
                .ThenBy(g => g.GridLayoutName);

            return Ok(await query.ToListAsync());
        }

        // GET: api/GridLayout/Default/ACCOUNT
        [HttpGet("Default/{voucherType}")]
        public async Task<IActionResult> GetDefault(string voucherType)
        {
            var layout = await FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                .FirstOrDefaultAsync(g => g.GridLayoutVoucherType == voucherType
                                       && g.GridLayoutDefault == true);

            if (layout == null) return NotFound(new { message = "No default layout found" });
            return Ok(layout);
        }

        // POST: api/GridLayout
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GridLayout dto)
        {
            try
            {
                dto.GridLayoutCompanyId = GetCompanyId();
                dto.GridLayoutAddedBy = GetUserId();
                dto.GridLayoutCreated = DateTime.UtcNow;

                // If this is default, make others non-default
                if (dto.GridLayoutDefault)
                {
                    var otherLayouts = await FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                        .Where(g => g.GridLayoutVoucherType == dto.GridLayoutVoucherType)
                        .ToListAsync();

                    foreach (var other in otherLayouts)
                    {
                        other.GridLayoutDefault = false;
                    }
                }

                _context.GridLayouts.Add(dto);
                await _context.SaveChangesAsync();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stack = ex.StackTrace });
            }
        }

        // PUT: api/GridLayout/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GridLayout dto)
        {
            var layout = await _context.GridLayouts.FindAsync(id);
            if (layout == null) return NotFound();

            // Update fields
            layout.GridLayoutName = dto.GridLayoutName;
            layout.GridLayoutVoucherType = dto.GridLayoutVoucherType;
            layout.GridLayoutVoucherId = dto.GridLayoutVoucherId;
            layout.GridLayoutData = dto.GridLayoutData;
            layout.GridLayoutDefault = dto.GridLayoutDefault;

            // Metadata
            layout.GridLayoutUpdateBy = GetUserId();
            layout.GridLayoutUpdated = DateTime.UtcNow;

            // If this is default, make others non-default
            if (dto.GridLayoutDefault)
            {
                var otherLayouts = await FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                    .Where(g => g.GridLayoutVoucherType == dto.GridLayoutVoucherType
                             && g.GridLayoutId != id)
                    .ToListAsync();

                foreach (var other in otherLayouts)
                {
                    other.GridLayoutDefault = false;
                }
            }

            _context.GridLayouts.Update(layout);
            await _context.SaveChangesAsync();
            return Ok(layout);
        }

        // PUT: api/GridLayout/SetDefault/5
        [HttpPut("SetDefault/{id}")]
        public async Task<IActionResult> SetDefault(int id)
        {
            try
            {
                var layout = await FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                    .FirstOrDefaultAsync(g => g.GridLayoutId == id);

                if (layout == null) return NotFound();

                // Make all other layouts of same type non-default
                var otherLayouts = await FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                    .Where(g => g.GridLayoutVoucherType == layout.GridLayoutVoucherType
                             && g.GridLayoutId != id)
                    .ToListAsync();

                foreach (var other in otherLayouts)
                {
                    other.GridLayoutDefault = false;
                }

                // Set this as default
                layout.GridLayoutDefault = true;
                layout.GridLayoutUpdateBy = GetUserId();
                layout.GridLayoutUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(layout);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stack = ex.StackTrace });
            }
        }

        // DELETE: api/GridLayout/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var layout = await FilterByCompany(_context.GridLayouts, "GridLayoutCompanyId")
                .FirstOrDefaultAsync(g => g.GridLayoutId == id);

            if (layout == null) return NotFound();

            _context.GridLayouts.Remove(layout);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}