using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.CurrencyDto;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : BaseController
    {
        private readonly AppDbContext _context;

        public CurrencyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyReadDto>>> GetAll()
        {
            var currencies = await FilterByCompany( _context.Currencies, "CurrencyCompanyId")
                .Select(c => new CurrencyReadDto
                {
                    CurrencyId = c.CurrencyId,
                    CurrencyUuid = c.CurrencyUuid,
                    CurrencyCompanyId = c.CurrencyCompanyId,
                    CurrencyName = c.CurrencyName,
                    CurrencySymbol = c.CurrencySymbol,
                    CurrencyStatus = c.CurrencyStatus,
                    CurrencyCreated = c.CurrencyCreated,
                    CurrencyUpdated = c.CurrencyUpdated
                }).ToListAsync();

            return Ok(currencies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyReadDto>> GetById(int id)
        {
            var c = await FilterByCompany( _context.Currencies, "CurrencyCompanyId").FirstOrDefaultAsync(b => b.CurrencyId == id);
            if (c == null) return NotFound();

            return new CurrencyReadDto
            {
                CurrencyId = c.CurrencyId,
                CurrencyUuid = c.CurrencyUuid,
                CurrencyCompanyId = c.CurrencyCompanyId,
                CurrencyName = c.CurrencyName,
                CurrencySymbol = c.CurrencySymbol,
                CurrencyStatus = c.CurrencyStatus,
                CurrencyCreated = c.CurrencyCreated,
                CurrencyUpdated = c.CurrencyUpdated
            };
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyReadDto>> Create(CurrencyCreateDto dto)
        {
            var currency = new Currency
            {
                CurrencyCompanyId = GetCompanyId(),
                CurrencyName = dto.CurrencyName,
                CurrencySymbol = dto.CurrencySymbol,
                CurrencyStatus = dto.CurrencyStatus,
                CurrencyCreated = DateTime.UtcNow,
                CurrencyUpdated = DateTime.UtcNow,
                CurrencyAddedByUserId = GetUserId(),
                CurrencyUpdatedByUserId = GetUserId()                 
               
            };

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = currency.CurrencyId }, new CurrencyReadDto
            {
                CurrencyId = currency.CurrencyId,
                CurrencyUuid = currency.CurrencyUuid,
                CurrencyCompanyId = currency.CurrencyCompanyId,
                CurrencyName = currency.CurrencyName,
                CurrencySymbol = currency.CurrencySymbol,
                CurrencyStatus = currency.CurrencyStatus,
                CurrencyCreated = currency.CurrencyCreated,
                CurrencyUpdated = currency.CurrencyUpdated
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CurrencyUpdateDto dto)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency == null) return NotFound();

            currency.CurrencyName = dto.CurrencyName;
            currency.CurrencySymbol = dto.CurrencySymbol;
            currency.CurrencyStatus = dto.CurrencyStatus;
            currency.CurrencyCompanyId = GetCompanyId();
            currency.CurrencyUpdatedByUserId = GetUserId();
            currency.CurrencyUpdated = DateTime.UtcNow;
         
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currency = await FilterByCompany(_context.Currencies, "CurrencyCompanyId").FirstOrDefaultAsync(b => b.CurrencyId == id);
            if (currency == null) return NotFound();

            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
