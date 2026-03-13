using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.DTOs.CurrencyDto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;

        public CurrencyController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyReadDto>>> GetAll()
        {
            var currencies = await FilterByCompany( _context.Currencies, "CurrencyCompanyId").OrderByDescending(b=>b.CurrencyId)
                .Select(c => new CurrencyReadDto
                {
                    CurrencyId = c.CurrencyId,
                    CurrencyUuid = c.CurrencyUuid,
                    CurrencyCompanyId = c.CurrencyCompanyId,
                    CurrencyName = c.CurrencyName,
                    CurrencySymbol = c.CurrencySymbol,
                    CurrencyStatus = c.CurrencyStatus,
                    CurrencyCreated = c.CurrencyCreated,
                    CurrencyUpdated = c.CurrencyUpdated
                }).ToListAsync();

            return Ok(currencies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyReadDto>> GetById(int id)
        {
            var c = await FilterByCompany( _context.Currencies, "CurrencyCompanyId").FirstOrDefaultAsync(b => b.CurrencyId == id);
            if (c == null) return NotFound();

            return new CurrencyReadDto
            {
                CurrencyId = c.CurrencyId,
                CurrencyUuid = c.CurrencyUuid,
                CurrencyCompanyId = c.CurrencyCompanyId,
                CurrencyName = c.CurrencyName,
                CurrencySymbol = c.CurrencySymbol,
                CurrencyStatus = c.CurrencyStatus,
                CurrencyCreated = c.CurrencyCreated,
                CurrencyUpdated = c.CurrencyUpdated
            };
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyReadDto>> Create(CurrencyCreateDto dto)
        {
            var exists = await _context.Currencies.AnyAsync(c =>
    c.CurrencyCompanyId == GetCompanyId() &&
    c.CurrencyName.Trim().ToLower() == dto.CurrencyName.Trim().ToLower()

);

            if (exists)
            {
                return BadRequest(new { message = "Currency Name already exists." });
            }
            var currency = new Currency
            {
                CurrencyCompanyId = GetCompanyId(),
                CurrencyName = dto.CurrencyName,
                CurrencySymbol = dto.CurrencySymbol,
                CurrencyStatus = dto.CurrencyStatus,
                CurrencyCreated = DateTime.UtcNow,
                CurrencyUpdated = DateTime.UtcNow,
                CurrencyAddedByUserId = GetUserId(),
                CurrencyUpdatedByUserId = GetUserId()                 
               
            };

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Currency",
                RecordId = currency.CurrencyId,
                VoucherType = "Currency",
                Amount = 0,
                Operations = "INSERT",
                Remarks = currency.CurrencyName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return CreatedAtAction(nameof(GetById), new { id = currency.CurrencyId }, new CurrencyReadDto
            {
                CurrencyId = currency.CurrencyId,
                CurrencyUuid = currency.CurrencyUuid,
                CurrencyCompanyId = currency.CurrencyCompanyId,
                CurrencyName = currency.CurrencyName,
                CurrencySymbol = currency.CurrencySymbol,
                CurrencyStatus = currency.CurrencyStatus,
                CurrencyCreated = currency.CurrencyCreated,
                CurrencyUpdated = currency.CurrencyUpdated
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CurrencyUpdateDto dto)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency == null) return NotFound();

            var exists = await _context.Currencies.AnyAsync(c =>
    c.CurrencyCompanyId == GetCompanyId() &&
    c.CurrencyId != id &&
    c.CurrencyName.Trim().ToLower() == dto.CurrencyName.Trim().ToLower() 
 
);

            if (exists)
            {
                return BadRequest(new { message = "Currency Name already exists." });

            }
            currency.CurrencyName = dto.CurrencyName;
            currency.CurrencySymbol = dto.CurrencySymbol;
            currency.CurrencyStatus = dto.CurrencyStatus;
            currency.CurrencyCompanyId = GetCompanyId();
            currency.CurrencyUpdatedByUserId = GetUserId();
            currency.CurrencyUpdated = DateTime.UtcNow;
         
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Currency",
                RecordId = currency.CurrencyId,
                VoucherType = "Currency",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = currency.CurrencyName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currency = await FilterByCompany(_context.Currencies, "CurrencyCompanyId").FirstOrDefaultAsync(b => b.CurrencyId == id);
            if (currency == null) return NotFound();
            // 🔎 Check used in Jobs
            bool usedInJobs = await _context.Jobs.AnyAsync(j =>
               j.JobDefCurrId == id &&
                j.JobCompanyId == GetCompanyId() &&
                j.JobActive == true
            );

            if (usedInJobs)
            {
                return BadRequest(new
                {
                    message = $"This Currency  is used in Jobs."
                });
            }
            // 🔎 Check used in Bills
            bool usedInBills = await _context.Bills.AnyAsync(b =>
                b.BillDefaultCurrencyId == id &&
                b.BillCompanyId == GetCompanyId() &&
                b.BillStatus == true
            );

            if (usedInBills)
            {
                return BadRequest(new
                {
                    message = $"This Currency  is used in Bills."
                });
            }
            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Currency",
                RecordId = id,
                VoucherType = "Currency",
                Amount = 0,
                Operations = "DELETE",
                Remarks = currency.CurrencyName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(true);
        }
    }
}
