using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.DTOs.LocationDto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LocationsController> _logger;
        private readonly AuditLogService _auditLogService;

        public LocationsController(AppDbContext context, ILogger<LocationsController> logger, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
            _context = context;
            _logger = logger;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
                var locations = await FilterByCompany(_context.Locations, "LocationCompanyId").OrderByDescending(B=>B.LocationId).ToListAsync();

                var result = locations.Select(l => new LocationReadDto
                {
                    LocationId = l.LocationId,
                    LocationName = l.LocationName,
                    LocationPincode = l.LocationPincode,
                    LocationState = l.LocationState,
                    LocationDistrict = l.LocationDistrict,
                    LocationStatus = l.LocationStatus,
                    LocationCode = l.LocationCode,
                    LocationUpdated = l.LocationUpdated,
                    LocationCreated = l.LocationCreated,
                    LocationCountryId = l.LocationCountryId,
                    LocationType = l.LocationType
                });

                return Ok(result);
            
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            
                var loc = await FilterByCompany(_context.Locations, "LocationCompanyId")
                                .FirstOrDefaultAsync(l => l.LocationId == id);

                if (loc == null) return NotFound(new { message = "Location not found" });

                var dto = new LocationReadDto
                {
                    LocationId = loc.LocationId,
                    LocationName = loc.LocationName,
                    LocationPincode = loc.LocationPincode,
                    LocationState = loc.LocationState,
                    LocationDistrict = loc.LocationDistrict,
                    LocationStatus = loc.LocationStatus,
                    LocationCode = loc.LocationCode,
                    LocationCountryId = loc.LocationCountryId,
                    LocationType = loc.LocationType
                };

                return Ok(dto);
            
        }

        // POST: api/Locations
        [HttpPost]
        public async Task<IActionResult> Create(LocationCreateDto dto)
        {
            
                var exists = await _context.Locations.AnyAsync(l =>
            l.LocationCompanyId == GetCompanyId() &&
            l.LocationType.Trim().ToLower() == dto.LocationType.Trim().ToLower() &&
            l.LocationName.Trim().ToLower() == dto.LocationName.Trim().ToLower()
        );

                if (exists)
                {
                    return BadRequest(new
                    {
                        message = $"Location  already exists in this '{dto.LocationType}'."
                    });
                }

                var location = new Location
                {
                    LocationName = dto.LocationName,
                    LocationPincode = dto.LocationPincode,
                    LocationState = dto.LocationState,
                    LocationDistrict = dto.LocationDistrict,
                    LocationStatus = dto.LocationStatus,
                    LocationCode = dto.LocationCode,
                    LocationCountryId = dto.LocationCountryId,
                    LocationType = dto.LocationType,
                    LocationCompanyId = GetCompanyId(),
                    LocationAddedByUserId = GetUserId(),
                    LocationUpdatedByUserId = GetUserId(),
                    LocationUpdated = DateTime.UtcNow,
                    LocationCreated = DateTime.UtcNow
                };

                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "Station/Port",
                    RecordId = location.LocationId,
                    VoucherType = "Station/Port",
                    Amount = 0,
                    Operations = "INSERT",
                    Remarks = location.LocationName,
                    BranchId = 0,
                    YearId = 0
                }, GetCompanyId());

                return Ok(location.LocationId);
            
        }

        // PUT: api/Locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LocationCreateDto dto)
        {
           
                var location = await FilterByCompany(_context.Locations, "LocationCompanyId")
                                    .FirstOrDefaultAsync(l => l.LocationId == id);

                if (location == null) return NotFound(new { message = "Location not found" });

                var exists = await _context.Locations.AnyAsync(l =>
            l.LocationCompanyId == GetCompanyId() &&
            l.LocationType.Trim().ToLower() == dto.LocationType.Trim().ToLower() &&
            l.LocationId != id &&
            l.LocationName.Trim().ToLower() == dto.LocationName.Trim().ToLower()
        );

                if (exists)
                {
                    return BadRequest(new
                    {
                        message = $"Location  already exists in this '{dto.LocationType}'."
                    });
                }
                location.LocationName = dto.LocationName;
                location.LocationPincode = dto.LocationPincode;
                location.LocationState = dto.LocationState;
                location.LocationDistrict = dto.LocationDistrict;
                location.LocationStatus = dto.LocationStatus;
                location.LocationCode = dto.LocationCode;
                location.LocationCountryId = dto.LocationCountryId;
                location.LocationType = dto.LocationType;
                location.LocationUpdatedByUserId = GetUserId();
                location.LocationUpdated = DateTime.UtcNow;
                location.LocationCompanyId = GetCompanyId();

                await _context.SaveChangesAsync();
                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "Station/Port",
                    RecordId = location.LocationId,
                    VoucherType = "Station/Port",
                    Amount = 0,
                    Operations = "UPDATE",
                    Remarks = location.LocationName,
                    BranchId = 0,
                    YearId = 0
                }, GetCompanyId());


                return NoContent();
            
            
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
                var location = await FilterByCompany(_context.Locations, "LocationCompanyId")
                                    .FirstOrDefaultAsync(l => l.LocationId == id);

                if (location == null) return NotFound(new { message = "Location not found" });

                // 🔎 Check used in Jobs
                bool usedInJobs = await _context.Jobs.AnyAsync(j =>
                   ( j.JobPodId == id || j.JobPolId == id)&&
                    j.JobCompanyId == GetCompanyId() &&
                    j.JobActive == true
                );

                if (usedInJobs)
                {
                    return BadRequest(new
                    {
                        message = $"This location '{location.LocationName}' is used in Jobs."
                    });
                }
                // 🔎 Check used in Bills
                bool usedInBills = await _context.Bills.AnyAsync(b =>
                    (b.BillPodId == id || b.BillPolId == id) &&
                    b.BillCompanyId == GetCompanyId() &&
                    b.BillStatus == true
                );

                if (usedInBills)
                {
                    return BadRequest(new
                    {
                        message = $"This location '{location.LocationName}' is used in Bills."
                    });
                }
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "Station/Port",
                    RecordId = id,
                    VoucherType = "Station/Port",
                    Amount = 0,
                    Operations = "DELETE",
                    Remarks = location.LocationName,
                    BranchId = 0,
                    YearId = 0
                }, GetCompanyId());


                return Ok(true);
           
        }
    }
}
