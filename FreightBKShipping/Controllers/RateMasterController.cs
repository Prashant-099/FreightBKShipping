using FreightBKShipping.Controllers;
using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RateMasterController : BaseController
{
    private readonly AppDbContext _context;
    public RateMasterController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        {
        var list = await _context.RateMasters
            .Include(r => r.Party)
            .Include(r => r.Service)
            .Select(r => new RateMaster
            {
                RateMasterId = r.RateMasterId,
                RateMasterPartyId = r.RateMasterPartyId,
                PartyName = r.Party.AccountName,
                RateMasterServiceId = r.RateMasterServiceId,
                ServiceName = r.Service.ServiceName,
                RateMasterSaleRate = r.RateMasterSaleRate,
                RateMasterPurchaseRate = r.RateMasterPurchaseRate,
                RateMasterApplicableDt = r.RateMasterApplicableDt
            })
            .ToListAsync();

        return Ok(list);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var rate = await _context.RateMasters.FirstOrDefaultAsync(r => r.RateMasterId == id);
        if (rate == null) return NotFound();
        return Ok(rate);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RateMasterCreateDto dto)
    {
        try
        {
            dto.RateMasterCreated = DateTime.UtcNow;
            dto.RateMasterUpdated = DateTime.UtcNow;
            dto.RateMasterAddedByUserId = GetUserId();
            dto.RateMasterUpdateByUserId = GetUserId(); 
            // Map DTO to Entity
            var rateMaster = new RateMaster
            {
                RateMasterPartyId = dto.RateMasterPartyId,
                RateMasterServiceId = dto.RateMasterServiceId,
                RateMasterSaleRate = dto.RateMasterSaleRate,
                RateMasterPurchaseRate = dto.RateMasterPurchaseRate,
                RateMasterApplicableDt = dto.RateMasterApplicableDt,

            };

            _context.RateMasters.Add(rateMaster);
            await _context.SaveChangesAsync();

            return Ok(rateMaster);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message, stack = ex.StackTrace });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RateMaster dto)
    {
        var rate = await _context.RateMasters.FindAsync(id);
        if (rate == null) return NotFound();

        // Copy updatable fields
        rate.RateMasterApplicableDt = dto.RateMasterApplicableDt;
        rate.RateMasterPartyId = dto.RateMasterPartyId;
        rate.RateMasterServiceId = dto.RateMasterServiceId;
        rate.RateMasterSaleRate = dto.RateMasterSaleRate;
        rate.RateMasterPurchaseRate = dto.RateMasterPurchaseRate;

        // Metadata
        rate.RateMasterUpdateByUserId = GetUserId();
        rate.RateMasterUpdated = DateTime.UtcNow;

        _context.RateMasters.Update(rate);
        await _context.SaveChangesAsync();
        return Ok(rate);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rate = await _context.RateMasters.FindAsync(id);
        if (rate == null) return NotFound();

        _context.RateMasters.Remove(rate);
        await _context.SaveChangesAsync();
        return Ok(true);
    }
}
