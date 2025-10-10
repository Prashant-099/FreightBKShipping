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
            var services = await FilterByCompany(_context.Services, "ServiceCompanyId")
                                .Include(s => s.ServiceGroup)
                                .ToListAsync();

            var result = services.Select(s => new ServiceReadDto
            {
                ServiceId = s.ServiceId,
                ServiceName = s.ServiceName,
                ServiceCode = s.ServiceCode,
                ServiceGroupId = s.ServiceGroupId,
                ServiceGroupsName = s.ServiceGroup != null ? s.ServiceGroup.ServiceGroupsName : null,
                ServiceStatus = s.ServiceStatus,
                ServiceSRate = s.ServiceSRate,
                ServicePRate = s.ServicePRate,
                ServiceRemarks = s.ServiceRemarks
            });

            return Ok(result);
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
                ServiceName = s.ServiceName,
                ServiceCode = s.ServiceCode,
                ServiceGroupId = s.ServiceGroupId,
                ServiceGroupsName = s.ServiceGroup?.ServiceGroupsName,
                ServiceStatus = s.ServiceStatus,
                ServiceSRate = s.ServiceSRate,
                ServicePRate = s.ServicePRate,
                ServiceRemarks = s.ServiceRemarks
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
