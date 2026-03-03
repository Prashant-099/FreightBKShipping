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
    public class StatusController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;

        public StatusController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;

            _context = context;
        }

        // GET: api/Status
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await  FilterByCompany( _context.Status, "StatusCompanyId")
                .OrderByDescending(s => s.StatusId)
                .ToListAsync();

            return Ok(statuses);
        }

        // GET: api/Status/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var status = await FilterByCompany(_context.Status, "StatusCompanyId").FirstOrDefaultAsync(a => a.StatusId == id);
            if (status == null) return NotFound();

            return Ok(status);
        }

        // POST: api/Status
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Status status)
        {
            status.StatusCreated = DateTime.UtcNow;
            status.StatusUpdated = DateTime.UtcNow;
            status.StatusCreatedByUser = GetUserId();
            status.StatusUpdatedByUser = GetUserId();
            status.StatusCompanyId = GetCompanyId();
            _context.Status.Add(status);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Status",
                RecordId = status.StatusId,
                VoucherType = "Status",
                Amount = 0,
                Operations = "INSERT",
                Remarks = $"{status.StatusName} | {status.Status_code}",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(status);
        }

        // PUT: api/Status/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Status model)
        {
            var status = await _context.Status.FindAsync(id);
            if (status == null) return NotFound();

            status.StatusName = model.StatusName;
            status.Status_code = model.Status_code;
            status.StatusUpdated = DateTime.UtcNow;
            status.StatusUpdatedByUser = GetUserId();
            status.StatusCompanyId = GetCompanyId();

            _context.Status.Update(status);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Status",
                RecordId = status.StatusId,
                VoucherType = "Status",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = $"{status.StatusName} | {status.Status_code}",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(status);
        }

        // DELETE: api/Status/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await FilterByCompany(_context.Status, "StatusCompanyId").FirstOrDefaultAsync(a => a.StatusId == id);
            if (status == null) return NotFound();

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Status",
                RecordId = id,
                VoucherType = "Status",
                Amount = 0,
                Operations = "DELETE",
                Remarks = $"{status.StatusName} | {status.Status_code}",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(true);
        }
    }
}
