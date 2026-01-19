using FreightBKShipping.Data;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.Models;

namespace FreightBKShipping.Services
{
    public class AuditLogService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public AuditLogService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task AddAsync(AuditLogCreateDto dto, int companyId)
        {
            var user = _http.HttpContext?.User?.Identity?.Name ?? "System";

            var log = new AuditLog
            {
                TableName = dto.TableName,
                RecordId = dto.RecordId,
                VoucherType = dto.VoucherType,
                Amount = dto.Amount,
                Operations = dto.Operations,
                Remarks = dto.Remarks,
                DateTime = DateTime.Now,
                CreatedBy = user,
                CompanyId = companyId,
                BranchId = dto.BranchId,
                YearId = dto.YearId
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
