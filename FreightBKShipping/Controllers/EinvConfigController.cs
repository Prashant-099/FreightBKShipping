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
    public class EinvConfigController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public EinvConfigController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
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
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "EinvoiceConfig",
                RecordId = int.Parse(model?.AspUserId??"0"),
                VoucherType = "EinvoiceConfig",
                Amount = 0,
                Operations = "INSERT",
                Remarks = model.Username,
                BranchId = model.BranchId,
                YearId = 0
            }, GetCompanyId());
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
            existing.eInvPwd = model.eInvPwd;
            existing.Gstin = model.Gstin;
            existing.AppKey = model.AppKey;
            existing.AuthToken = model.AuthToken;
            existing.Sek = model.Sek;
            existing.EInvoiceTokenExp = model.EInvoiceTokenExp;
            existing.BranchId = model.BranchId;
            existing.GspName = model.GspName;
            existing.AspUserId = model.AspUserId;
            existing.Password = model.Password;
            existing.AuthUrl = model.AuthUrl;
            existing.EwbByIrn = model.EwbByIrn;
            existing.EwbUrl = model.EwbUrl;
            existing.CancelEwbUrl = model.CancelEwbUrl;
            existing.BaseUrl = model.BaseUrl;

            _context.EinvConfigs.Update(existing);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "EinvoiceConfig",
                RecordId = int.Parse(model?.AspUserId ?? "0"),
                VoucherType = "EinvoiceConfig",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = model.Username,
                BranchId = model.BranchId,
                YearId = 0
            }, GetCompanyId());
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
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "EinvoiceConfig",
                RecordId = int.Parse(record?.AspUserId ?? "0"),
                VoucherType = "EinvoiceConfig",
                Amount = 0,
                Operations = "DELETE",
                Remarks = record.Username,
                BranchId = record.BranchId,
                YearId = 0
            }, GetCompanyId());
            _context.EinvConfigs.Remove(record);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
