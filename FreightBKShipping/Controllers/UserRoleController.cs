using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreightBKShipping.Data;
using FreightBKShipping.Models;
using FreightBKShipping.DTOs.UserRole;
using Microsoft.AspNetCore.Authorization;
using Sieve.Services;
using Sieve.Models;

namespace FreightBKShipping.Controllers
{
    [Authorize]
    [ApiController]
    
    [Route("api/[controller]")]
    public class UserRoleController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        public UserRoleController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        // ✅ GET: api/userroles
        [HttpGet]
        public async Task<ActionResult> GetAllRoles([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany( _context.UserRoles.AsNoTracking(), "RoleCompanyId");

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedRoles = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new UserRoleDto
                {
                    RoleUuid = r.RoleUuid,
                    RoleName = r.RoleName,
                    RoleStatus = r.RoleStatus,
                    RoleCreated = r.RoleCreated,
                    RoleUpdated = r.RoleUpdated,
                    RoleCompanyId = r.RoleCompanyId
                })
                .ToListAsync();

            return Ok(new
            {
                pagination = new
                {
                    page = currentPage,
                    pageSize = pageSize,
                    totalRecords = totalRecords,
                    totalPages = totalPages
                },
                pagedRoles
            });
        }


        // ✅ GET: api/userroles/{uuid}
        [HttpGet("{uuid}")]
        public async Task<ActionResult<UserRoleDto>> GetRoleByUuid(string uuid)
        {
            var role = await  FilterByCompany( _context.UserRoles, "RoleCompanyId").FirstOrDefaultAsync(r => r.RoleUuid == uuid);
            if (role == null)
                return NotFound();

            var result = new UserRoleDto
            {
                RoleUuid = role.RoleUuid,
                RoleName = role.RoleName,
                RoleStatus = role.RoleStatus,
                RoleCreated = role.RoleCreated,
                RoleUpdated = role.RoleUpdated,
                RoleCompanyId = role.RoleCompanyId
            };

            return Ok(result);
        }

        // ✅ POST: api/userroles
        [HttpPost]
        public async Task<ActionResult<UserRoleDto>> CreateRole(UserRoleCreateDto dto)
        {
            var role = new UserRole
            {
                RoleUuid = Guid.NewGuid().ToString(),
                RoleCompanyId = GetCompanyId(),
                RoleAddedByUserId =GetUserId(),
                RoleUpdatedByUserId =GetUserId(),
                RoleName = dto.RoleName,
                RoleStatus = 1,
                RoleCreated = DateTime.UtcNow,
                RoleUpdated = DateTime.UtcNow
            };

            _context.UserRoles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoleByUuid), new { uuid = role.RoleUuid }, new UserRoleDto
            {
                RoleUuid = role.RoleUuid,
                RoleName = role.RoleName,
                RoleStatus = role.RoleStatus,
                RoleCreated = DateTime.UtcNow,
                RoleUpdated = DateTime.UtcNow
            });
        }

        // ✅ PUT: api/userroles/{uuid}
        [HttpPut("{uuid}")]
        public async Task<IActionResult> UpdateRole(string uuid, UserRoleUpdateDto dto)
        {
            if (uuid != dto.RoleUuid)
                return BadRequest("UUID mismatch");

            var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.RoleUuid == uuid);
            if (role == null)
                return NotFound();
            role.RoleCompanyId = GetCompanyId();
            role.RoleName = dto.RoleName;
            role.RoleUpdated = DateTime.UtcNow;
            role.RoleStatus = dto.RoleStatus;
            role.RoleUpdatedByUserId = GetUserId();
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE: api/userroles/{uuid}
        [HttpDelete("{uuid}")]
        public async Task<IActionResult> DeleteRole(string uuid)
        {
            var role = await FilterByCompany( _context.UserRoles, "RoleCompanyId").FirstOrDefaultAsync(r => r.RoleUuid == uuid);
            if (role == null)
                return NotFound();

            _context.UserRoles.Remove(role);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
