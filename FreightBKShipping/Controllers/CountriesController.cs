using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Country;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : BaseController
    {
        private readonly AppDbContext _context;

        public CountriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryReadDto>>> GetAll()
        {
            var countries = await FilterByCompany(_context.Countries, "CountryCompanyId").OrderByDescending(b=>b.CountryId)
                  .Select(c => new CountryReadDto
                {
                    CountryId = c.CountryId,
                    CountryCompanyId = c.CountryCompanyId,
                    CountryName = c.CountryName,
                    CountryCode = c.CountryCode,
                    CountryCurrency = c.CountryCurrency,
                    CountryRemarks = c.CountryRemarks,
                    CountryStatus = c.CountryStatus,
                    CountryCreated = c.CountryCreated,
                    CountryUpdated = c.CountryUpdated
                }).ToListAsync();

            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryReadDto>> GetById(int id)
        {
            var c = await FilterByCompany( _context.Countries, "CountryCompanyId").FirstOrDefaultAsync(b => b.CountryId == id);
            if (c == null) return NotFound();

            return new CountryReadDto
            {
                CountryId = c.CountryId,
                CountryCompanyId = c.CountryCompanyId,
                CountryName = c.CountryName,
                CountryCode = c.CountryCode,
                CountryCurrency = c.CountryCurrency,
                CountryRemarks = c.CountryRemarks,
                CountryStatus = c.CountryStatus,
                CountryCreated = c.CountryCreated,
                CountryUpdated = c.CountryUpdated
            };
        }

        [HttpPost]
        public async Task<ActionResult<CountryReadDto>> Create(CountryCreateDto dto)
        {
            var exists = await _context.Countries.AnyAsync(c =>
    c.CountryCompanyId == GetCompanyId() &&
    c.CountryName.Trim().ToLower() == dto.CountryName.Trim().ToLower()
);

            if (exists)
            {
                return BadRequest("Country Already Exists.");
            }
            var country = new Country
            {
                CountryCompanyId = GetCompanyId(),
                CountryUpdatedbyUserId =GetUserId(),
                CountryAddbyUserId =GetUserId(),
                CountryName = dto.CountryName,
                CountryCode = dto.CountryCode,
                CountryCurrency = dto.CountryCurrency,
                CountryRemarks = dto.CountryRemarks,
                CountryStatus = dto.CountryStatus,
                CountryCreated = DateTime.UtcNow,
                CountryUpdated = DateTime.UtcNow
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = country.CountryId }, new CountryReadDto
            {
                CountryId = country.CountryId,
                CountryCompanyId = country.CountryCompanyId,
                CountryName = country.CountryName,
                CountryCode = country.CountryCode,
                CountryCurrency = country.CountryCurrency,
                CountryRemarks = country.CountryRemarks,
                CountryStatus = country.CountryStatus,
                CountryCreated = country.CountryCreated,
                CountryUpdated = country.CountryUpdated
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CountryUpdateDto dto)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            var exists = await _context.Countries.AnyAsync(c =>
    c.CountryCompanyId == GetCompanyId() &&
    c.CountryId != id &&
    c.CountryName.Trim().ToLower() == dto.CountryName.Trim().ToLower()
);

            if (exists)
            {
                return BadRequest(new { message = "Country Already Exists." });
            }
            country.CountryName = dto.CountryName;
            country.CountryCode = dto.CountryCode;
            country.CountryCurrency = dto.CountryCurrency;
            country.CountryRemarks = dto.CountryRemarks;
            country.CountryStatus = dto.CountryStatus;
            country.CountryUpdated = DateTime.UtcNow;
            country.CountryCompanyId = GetCompanyId();
            country.CountryUpdatedbyUserId = GetUserId();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await FilterByCompany(_context.Countries, "CountryCompanyId").FirstOrDefaultAsync(b => b.CountryId == id) ;
            if (country == null) return NotFound();

            // 🔎 Check used in Jobs
            bool usedInJobs = await _context.Jobs.AnyAsync(j =>
               j.JobCountryOrigin.Trim().ToLower() == country.CountryName.Trim().ToLower() &&
                j.JobCompanyId == GetCompanyId() &&
                j.JobActive == true
            );

            if (usedInJobs)
            {
                return BadRequest( $"It is used in Jobs.");
            }

            // 🔎 Check used in Jobs
            bool usedInlocation = await _context.Locations.AnyAsync(j =>
               j.LocationCountryId== id &&
                j.LocationCompanyId == GetCompanyId() &&
                j.LocationStatus == true
            );

            if (usedInlocation)
            {
                return BadRequest( $"It is used in Station/Port.");
            }
            _context.Countries.Remove(country);
          
                await _context.SaveChangesAsync();
                return Ok(true);
           
            
        }
    }
}
