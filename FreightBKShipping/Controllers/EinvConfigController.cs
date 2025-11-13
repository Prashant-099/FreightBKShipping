using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EinvConfigController : BaseController
    {
        private readonly AppDbContext _context;

        public EinvConfigController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/EinvConfig
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await FilterByCompany(_context.EinvConfigs, "CompanyId").ToListAsync();
            return Ok(list);
        }

        // ✅ GET: api/EinvConfig/{username}
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var record = await FilterByCompany(_context.EinvConfigs, "CompanyId")
                                .FirstOrDefaultAsync(x => x.Username == username);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        // ✅ POST: api/EinvConfig
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EinvConfig model)
        {
            if (model == null)
                return BadRequest("Invalid data");

            // prevent duplicate for same company
            bool exists = await _context.EinvConfigs
                .AnyAsync(x => x.Username == model.Username && x.CompanyId == GetCompanyId());
            if (exists)
                return BadRequest("Configuration with same username already exists.");

            model.CompanyId = GetCompanyId();
            model.BranchId = model.BranchId ?? 0;

            _context.EinvConfigs.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        // ✅ PUT: api/EinvConfig/{username}
        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username, [FromBody] EinvConfig model)
        {
            if (username != model.Username)
                return BadRequest("Username mismatch");

            var existing = await FilterByCompany(_context.EinvConfigs, "CompanyId")
                                    .FirstOrDefaultAsync(x => x.Username == username);

            if (existing == null)
                return NotFound();

            // Update all editable fields
            existing.Password = model.Password;
            existing.Gstin = model.Gstin;
            existing.AppKey = model.AppKey;
            existing.AuthToken = model.AuthToken;
            existing.Sek = model.Sek;
            existing.EInvoiceTokenExp = model.EInvoiceTokenExp;
            existing.BranchId = model.BranchId;
            existing.GspName = model.GspName;
            existing.AspUserId = model.AspUserId;
            existing.AspPassword = model.AspPassword;
            existing.AuthUrl = model.AuthUrl;
            existing.EwbByIrn = model.EwbByIrn;
            existing.EwbUrl = model.EwbUrl;
            existing.CancelEwbUrl = model.CancelEwbUrl;
            existing.BaseUrl = model.BaseUrl;

            _context.EinvConfigs.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // ✅ DELETE: api/EinvConfig/{username}
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var record = await FilterByCompany(_context.EinvConfigs, "CompanyId")
                                .FirstOrDefaultAsync(x => x.Username == username);

            if (record == null)
                return NotFound();

            _context.EinvConfigs.Remove(record);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
