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
        public class YearsController : BaseController
    {
            private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;
        public YearsController(AppDbContext context, AuditLogService auditLogService)
            {
            _auditLogService = auditLogService;
            _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Year>>> GetYears()
            {
            return await FilterByCompany(_context.Years.AsNoTracking(), "YearCompanyId").OrderByDescending(b => b.YearId).ToListAsync();
        }

            [HttpGet("{id}")]
            public async Task<ActionResult<Year>> GetYear(int id)
            {
                var year = await FilterByCompany(_context.Years.AsNoTracking(), "YearCompanyId")
        .FirstOrDefaultAsync(y => y.YearId == id);
            if (year == null)
                    return NotFound();

                return year;
            }

            [HttpPost]
            public async Task<ActionResult<Year>> CreateYear(Year year)
        {
           

                var isDuplicate = await _context.Years
    .AnyAsync(y =>
        y.YearCompanyId == GetCompanyId() &&
         y.YearDateFrom == year.YearDateFrom.Value.Date &&
        y.YearDateTo == year.YearDateTo.Value.Date
    );

            if (isDuplicate)
            {
                return BadRequest("Financial Year Already Exists.");
            }

            year.YearCompanyId =GetCompanyId();
            year.YearCreated = DateTime.UtcNow;
                year.YearUpdated = DateTime.UtcNow;
            year.YearAddByUserId = GetUserId();
            year.YearUpdatedByUserId = GetUserId();

            // If this is being set as default, unset all others for the same company
            if (year.YearIsDefault)
            {
                var existingDefaults = await _context.Years
                    .Where(y => y.YearCompanyId == year.YearCompanyId && y.YearIsDefault)
                    .ToListAsync();

                foreach (var y in existingDefaults)
                {
                    y.YearIsDefault = false;
                }
            }
            _context.Years.Add(year);
                await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Year",
                RecordId = year.YearId,
                VoucherType = "Year",
                Amount = 0,
                Operations = "INSERT",
                Remarks = year.YearName,
                BranchId = 0,
                YearId = year.YearId
            }, GetCompanyId());
            return CreatedAtAction(nameof(GetYear), new { id = year.YearId }, year);
           
        }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateYear(int id, Year year)
            {
          
                if (id != year.YearId)
                    return BadRequest();
            var isDuplicate = await _context.Years
.AnyAsync(y =>
    y.YearId != id &&
    y.YearCompanyId == GetCompanyId() &&
                y.YearDateFrom == year.YearDateFrom.Value.Date &&
    y.YearDateTo == year.YearDateTo.Value.Date
);

            if (isDuplicate)
            {
                return BadRequest(new
                {
                    message = "Financial Year Already Exists."
                });
            }

            year.YearCompanyId = GetCompanyId();
                year.YearUpdated = DateTime.UtcNow;
            year.YearUpdatedByUserId = GetUserId();
            year.YearAddByUserId = GetUserId();
            // If this is being set as default, unset all others for the same company
            if (year.YearIsDefault)
            {
                var existingDefaults = await _context.Years
                    .Where(y => y.YearCompanyId == year.YearCompanyId && y.YearId != year.YearId && y.YearIsDefault)
                    .ToListAsync();

                foreach (var y in existingDefaults)
                {
                    y.YearIsDefault = false;
                }
            }
            _context.Entry(year).State = EntityState.Modified;

                
                    await _context.SaveChangesAsync();
                
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Year",
                RecordId = year.YearId,
                VoucherType = "Year",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = year.YearName,
                BranchId = 0,
                YearId = year.YearId
            }, GetCompanyId());
            return NoContent();
            
        }   

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteYear(int id)
            {
                var year = await FilterByCompany( _context.Years, "YearCompanyId").FirstOrDefaultAsync(b => b.YearId == id); 
                if (year == null)
                    return NotFound();

            bool usedInJob = await _context.Jobs.AnyAsync(s =>
          s.JobYearId == id.ToString() &&
          s.JobActive == true &&
          s.JobCompanyId == GetCompanyId()
      );

            if (usedInJob)
            {
                return BadRequest($"This Year is used in Jobs.");
            }
            bool usedInbill = await _context.Bills.AnyAsync(s =>
               s.BillYearId == id &&
               s.BillStatus == true &&
               s.BillCompanyId == GetCompanyId()
           );

            if (usedInbill)
            {
                return BadRequest( $"This Year is used in Bills.");
            }
            bool usedInjournal = await _context.Journals.AnyAsync(s =>
               s.JournalYearId == id &&
               s.JournalCompanyId == GetCompanyId()
           );
            if (usedInjournal)
            {
                return BadRequest( $"This Year is used in Payment/Receipt/journal/Contra.");
            }

            bool usedInVoucher = await _context.VoucherDetails.AnyAsync(s =>
         s.VoucherDetailYearId == id &&
               s.Voucher.VoucherCompanyId == GetCompanyId()
     );

            if (usedInVoucher)
            {
                return BadRequest( $"This Year is used in Voucher Type.");
            }
            _context.Years.Remove(year);
                await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Year",
                RecordId = id,
                VoucherType = "Year",
                Amount = 0,
                Operations = "DELETE",
                Remarks = year.YearName,
                BranchId = 0,
                YearId = id
            }, GetCompanyId());
            return Ok(true);
            }
        }
    }

