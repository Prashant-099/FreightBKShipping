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
        public async Task<IActionResult> GetLatest()
        {
            var query = from log in FilterByCompany(_context.AuditLogs, "CompanyId")
                        join branch in _context.Branches
                            on log.BranchId equals branch.BranchId into branchJoin
                        from branch in branchJoin.DefaultIfEmpty()
                        orderby log.DateTime descending
                        select new AuditLogReadDto
                        {
                            AuditLogsId = log.AuditLogsId,
                            TableName = log.TableName,
                            RecordId = (int)log.RecordId,
                            VoucherType = log.VoucherType,
                            Amount = (int)log.Amount,
                            Operations = log.Operations,
                            Remarks = log.Remarks,
                            DateTime = (DateTime)log.DateTime,
                            CreatedBy = log.CreatedBy,
                            CompanyId = (int)log.CompanyId,
                            YearId = (int)log.YearId,
                            BranchId = log.BranchId,
                            BranchName = branch != null ? branch.BranchName : "All"
                        };

            var list = await query.ToListAsync();
            return Ok(list);
        }
    }

}
