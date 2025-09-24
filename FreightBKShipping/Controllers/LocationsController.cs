using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.LocationDto;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : BaseController
    {
        private readonly AppDbContext _context;

        public LocationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await FilterByCompany(_context.Locations, "LocationCompanyId").ToListAsync();

            var result = locations.Select(l => new LocationReadDto
            {
                LocationId = l.LocationId,
                LocationName = l.LocationName,
                LocationPincode = l.LocationPincode,
                LocationState = l.LocationState,
                LocationDistrict = l.LocationDistrict,
                LocationStatus = l.LocationStatus,
                LocationCode = l.LocationCode,
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

            if (loc == null) return NotFound("Location not found");

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
                LocationUpdatedByUserId =GetUserId(),
                LocationUpdated = DateTime.UtcNow,
                LocationCreated = DateTime.UtcNow
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return Ok(location.LocationId);
        }

        // PUT: api/Locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LocationCreateDto dto)
        {
            var location = await FilterByCompany(_context.Locations, "LocationCompanyId")
                                .FirstOrDefaultAsync(l => l.LocationId == id);

            if (location == null) return NotFound("Location not found");

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
            return NoContent();
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await FilterByCompany(_context.Locations, "LocationCompanyId")
                                .FirstOrDefaultAsync(l => l.LocationId == id);

            if (location == null) return NotFound("Location not found");

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
