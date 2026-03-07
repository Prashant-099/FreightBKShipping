using FreightBKShipping.Data;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [ApiController]
    [Route("api/audit-log")]
    public class AuditLogController : BaseController
    {
        private readonly AuditLogService _service;
        private readonly AppDbContext _context;

        public AuditLogController(AuditLogService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(AuditLogCreateDto dto)
        //{
        //    await _service.AddAsync(dto, GetCompanyId());
        //    return Ok();
        //}

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest(
      DateTime? fromDate,
      DateTime? toDate)
        {
            // ✅ Default: Last 1 Months
            if (!fromDate.HasValue && !toDate.HasValue)
            {
                toDate = DateTime.Today;
                fromDate = toDate.Value.AddMonths(-1);

            }
            var query = from log in FilterByCompany(_context.AuditLogs, "CompanyId")
                        join branch in _context.Branches
                            on log.BranchId equals branch.BranchId into branchJoin
                        from branch in branchJoin.DefaultIfEmpty()
                        select new
                        {
                            log,
                            BranchName = branch != null ? branch.BranchName : "All"
                        };

            // ✅ From Date Filter
            if (fromDate.HasValue)
                query = query.Where(x => x.log.DateTime >= fromDate.Value.Date);

            // ✅ To Date Filter (inclusive)
            if (toDate.HasValue)
            {   
                var endDate = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.log.DateTime < endDate);
            }

            var list = await query
                .OrderByDescending(x => x.log.DateTime)
                .Select(x => new AuditLogReadDto
                {
                    AuditLogsId = x.log.AuditLogsId,
                    TableName = x.log.TableName,
                    RecordId = (int)x.log.RecordId,
                    VoucherType = x.log.VoucherType,
                    Amount = (int)x.log.Amount,
                    Operations = x.log.Operations,
                    Remarks = x.log.Remarks,
                    DateTime = (DateTime)x.log.DateTime,
                    CreatedBy = x.log.CreatedBy,
                    CompanyId = (int)x.log.CompanyId,
                    YearId = (int)x.log.YearId,
                    BranchId = x.log.BranchId,
                    BranchName = x.BranchName
                })
                .ToListAsync();

            return Ok(list);
        }
    }

}
