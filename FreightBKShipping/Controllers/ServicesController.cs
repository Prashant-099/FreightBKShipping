using FreightBKShipping.Data;
using FreightBKShipping.DTOs.ServiceDto;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : BaseController
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Services
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var query = FilterByCompany(_context.Services, "ServiceCompanyId")
         .GroupJoin(_context.ServiceGroups,
             s => s.ServiceGroupId,
             g => g.ServiceGroupsId,
             (s, g) => new { s, g })
         .SelectMany(
             sg => sg.g.DefaultIfEmpty(),
             (sg, g) => new { sg.s, g })
         .GroupJoin(_context.HsnSacs,
             sg => sg.s.ServiceHsnId,
             h => h.HsnId,
             (sg, h) => new { sg.s, sg.g, h })
         .SelectMany(
             sgh => sgh.h.DefaultIfEmpty(),
             (sgh, h) => new { sgh.s, sgh.g, h })
         .GroupJoin(_context.Accounts,
             sgh => sgh.s.ServiceAccountId,
             a => a.AccountId,
             (sgh, a) => new { sgh.s, sgh.g, sgh.h, a })
         .SelectMany(
             sgha => sgha.a.DefaultIfEmpty(),
             (sgha, a) => new { sgha.s, sgha.g, sgha.h, a });

            var result = await query.Select(x => new ServiceReadDto
            {
                ServiceId = x.s.ServiceId,
                ServiceCompanyId = x.s.ServiceCompanyId,
                ServiceGroupId = x.s.ServiceGroupId,
                ServiceUnitId = x.s.ServiceUnitId,
                ServiceName = x.s.ServiceName,
                ServiceCode = x.s.ServiceCode,
                ServiceType = x.s.ServiceType,
                ServiceSRate = x.s.ServiceSRate,
                ServicePRate = x.s.ServicePRate,
                ServiceChargeType = x.s.ServiceChargeType,
                ServiceHsnId = x.s.ServiceHsnId,
                ServiceExempt = x.s.ServiceExempt,
                ServiceRemarks = x.s.ServiceRemarks,
                ServicePrintName = x.s.ServicePrintName,
                ServiceTallyName = x.s.ServiceTallyName,
                ServiceStatus = x.s.ServiceStatus,
                ServiceExtraCharge = x.s.ServiceExtraCharge,
                ServiceCeilingType = x.s.ServiceCeilingType,
                ServiceCeilingValue = x.s.ServiceCeilingValue,
                ServiceVoucherId = x.s.ServiceVoucherId,
                ServiceAccountId = x.s.ServiceAccountId,
                ServiceIsGoods = x.s.ServiceIsGoods,
                ServiceAddedByUserId = x.s.ServiceAddedByUserId,
                ServiceUpdatedByUserId = x.s.ServiceUpdatedByUserId,
                ServiceCreated = x.s.ServiceCreated,
                ServiceUpdated = x.s.ServiceUpdated,

                // ✅ Safe fields
                GroupName = x.g != null ? x.g.ServiceGroupsName : null,
                HsnName = x.h != null ? x.h.HsnName : null,
                HsnGstPer = x.h != null ? x.h.HsnGstPer : 0,
                AccountName = x.a != null ? x.a.AccountName : null
            }).ToListAsync();

            return Ok(result); // ✅ FIXED
        }

        // ✅ GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var s = await FilterByCompany(_context.Services, "ServiceCompanyId")
                                .Include(s => s.ServiceGroup)
                                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (s == null) return NotFound("Service not found");

            var dto = new ServiceReadDto
            {
                ServiceId = s.ServiceId,
                ServiceCompanyId = s.ServiceCompanyId,
                ServiceGroupId = s.ServiceGroupId,
                ServiceUnitId = s.ServiceUnitId,

                ServiceName = s.ServiceName,
                ServiceCode = s.ServiceCode,
                ServiceType = s.ServiceType,

                ServiceSRate = s.ServiceSRate,
                ServicePRate = s.ServicePRate,

                ServiceChargeType = s.ServiceChargeType,
                ServiceHsnId = s.ServiceHsnId,
                ServiceExempt = s.ServiceExempt,

                ServiceRemarks = s.ServiceRemarks,
                ServicePrintName = s.ServicePrintName,
                ServiceTallyName = s.ServiceTallyName,

                ServiceStatus = s.ServiceStatus,
                ServiceExtraCharge = s.ServiceExtraCharge,
                ServiceCeilingType = s.ServiceCeilingType,
                ServiceCeilingValue = s.ServiceCeilingValue,

                ServiceVoucherId = s.ServiceVoucherId,
                ServiceAccountId = s.ServiceAccountId,
                ServiceIsGoods = s.ServiceIsGoods,

                ServiceAddedByUserId = s.ServiceAddedByUserId,
                ServiceUpdatedByUserId = s.ServiceUpdatedByUserId,

                ServiceCreated = s.ServiceCreated,
                ServiceUpdated = s.ServiceUpdated
            };

            return Ok(dto);
        }

        // ✅ POST: api/Services
        [HttpPost]
        public async Task<IActionResult> Create(ServiceCreateDto dto)
        {
            // Optional: prevent duplicate service name within company
            bool exists = await _context.Services
                .AnyAsync(s => s.ServiceCompanyId == GetCompanyId() && s.ServiceName == dto.ServiceName);
            if (exists)
                return Conflict($"Service '{dto.ServiceName}' already exists for this company.");

            var service = new Service
            {
                ServiceCompanyId = GetCompanyId(),
                ServiceAddedByUserId = GetUserId(),
                ServiceUpdatedByUserId = GetUserId(),
                ServiceName = dto.ServiceName,
                ServiceCode = dto.ServiceCode,
                ServiceGroupId = dto.ServiceGroupId,
                ServiceSRate = dto.ServiceSRate,
                ServicePRate = dto.ServicePRate,
                ServiceStatus = dto.ServiceStatus,
                ServiceRemarks = dto.ServiceRemarks,
                ServiceType = dto.ServiceType,
                ServiceUnitId = dto.ServiceUnitId,
                ServiceCreated = DateTime.UtcNow,
                ServiceUpdated = DateTime.UtcNow
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return Ok(new { service.ServiceId });
        }

        // ✅ PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceCreateDto dto)
        {
            var service = await FilterByCompany(_context.Services, "ServiceCompanyId")
                                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (service == null)
                return NotFound("Service not found");

            service.ServiceName = dto.ServiceName;
            service.ServiceCode = dto.ServiceCode;
            service.ServiceGroupId = dto.ServiceGroupId;
            service.ServiceSRate = dto.ServiceSRate;
            service.ServicePRate = dto.ServicePRate;
            service.ServiceStatus = dto.ServiceStatus;
            service.ServiceRemarks = dto.ServiceRemarks;
            service.ServiceUnitId = dto.ServiceUnitId;
            service.ServiceType = dto.ServiceType;
            service.ServiceUpdatedByUserId = GetUserId();
            service.ServiceUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await FilterByCompany(_context.Services, "ServiceCompanyId")
                                .FirstOrDefaultAsync(s => s.ServiceId == id);

            if (service == null)
                return NotFound("Service not found");

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
