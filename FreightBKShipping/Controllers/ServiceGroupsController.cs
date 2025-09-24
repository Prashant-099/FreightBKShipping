using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.ServiceGroup;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceGroupsController : BaseController
    {
        private readonly AppDbContext _context;

        public ServiceGroupsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceGroups
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await FilterByCompany(_context.ServiceGroups, "ServiceGroupsCompanyId").ToListAsync();

            var result = groups.Select(g => new ServiceGroupReadDto
            {
                ServiceGroupsId = g.ServiceGroupsId,
                ServiceGroupsName = g.ServiceGroupsName,
                ServiceGroupsStatus = g.ServiceGroupsStatus,
                ServiceGroupsRemarks = g.ServiceGroupsRemarks
            });

            return Ok(result);
        }

        // GET: api/ServiceGroups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var group = await FilterByCompany(_context.ServiceGroups, "ServiceGroupsCompanyId")
                              .FirstOrDefaultAsync(g => g.ServiceGroupsId == id);

            if (group == null) return NotFound("Service group not found");

            var dto = new ServiceGroupReadDto
            {
                ServiceGroupsId = group.ServiceGroupsId,
                ServiceGroupsName = group.ServiceGroupsName,
                ServiceGroupsStatus = group.ServiceGroupsStatus,
                ServiceGroupsRemarks = group.ServiceGroupsRemarks
            };

            return Ok(dto);
        }

        // POST: api/ServiceGroups
        [HttpPost]
        public async Task<IActionResult> Create(ServiceGroupCreateDto dto)
        {
            var group = new ServiceGroup
            {
                ServiceGroupsName = dto.ServiceGroupsName,
                ServiceGroupsStatus = dto.ServiceGroupsStatus,
                ServiceGroupsRemarks = dto.ServiceGroupsRemarks,
                ServiceGroupsAddedByUserId = GetUserId(),
                ServiceGroupsCompanyId = GetCompanyId(),
                ServiceGroupsAdded = DateTime.UtcNow,
                ServiceGroupsUpdated= DateTime.UtcNow
            };

            _context.ServiceGroups.Add(group);
            await _context.SaveChangesAsync();

            return Ok(group.ServiceGroupsId);
        }

        // PUT: api/ServiceGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ServiceGroupCreateDto dto)
        {
            var group = await FilterByCompany(_context.ServiceGroups, "ServiceGroupsCompanyId")
                              .FirstOrDefaultAsync(g => g.ServiceGroupsId == id);

            if (group == null) return NotFound("Service group not found");

            group.ServiceGroupsName = dto.ServiceGroupsName;
            group.ServiceGroupsStatus = dto.ServiceGroupsStatus;
            group.ServiceGroupsRemarks = dto.ServiceGroupsRemarks;
            group.ServiceGroupsUpdatedByUserId = GetUserId();
            group.ServiceGroupsUpdated = DateTime.UtcNow;
            group.ServiceGroupsCompanyId = GetCompanyId();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ServiceGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await FilterByCompany(_context.ServiceGroups, "ServiceGroupsCompanyId")
                              .FirstOrDefaultAsync(g => g.ServiceGroupsId == id);

            if (group == null) return NotFound("Service group not found");

            _context.ServiceGroups.Remove(group);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
