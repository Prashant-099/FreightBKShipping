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

    // 🔹 GET ALL (COMPANY FILTERED)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var companyId = GetCompanyId();

        var list = await _context.RateMasters
            .Where(r => r.RateMasterCompanyId == companyId)
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
                RateMasterUpdated = r.RateMasterUpdated,
                RateMasterCreated = r.RateMasterCreated,
                RateMasterApplicableDt = r.RateMasterApplicableDt
            })
            .OrderByDescending(b => b.RateMasterId)
            .ToListAsync();

        return Ok(list);
    }

    // 🔹 GET BY ID (COMPANY FILTERED)
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var companyId = GetCompanyId();

        var rate = await _context.RateMasters
            .FirstOrDefaultAsync(r =>
                r.RateMasterId == id &&
                r.RateMasterCompanyId == companyId);

        if (rate == null)
            return NotFound();

        return Ok(rate);
    }

    // 🔹 CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RateMasterCreateDto dto)
    {
        try
        {
            var rateMaster = new RateMaster
            {
                RateMasterPartyId = dto.RateMasterPartyId,
                RateMasterServiceId = dto.RateMasterServiceId,
                RateMasterSaleRate = dto.RateMasterSaleRate,
                RateMasterPurchaseRate = dto.RateMasterPurchaseRate,
                RateMasterApplicableDt = dto.RateMasterApplicableDt,

                // 🔐 Company + audit
                RateMasterCompanyId = GetCompanyId(),
                RateMasterAddedByUserId = GetUserId(),
                RateMasterUpdateByUserId = GetUserId(),
                RateMasterCreated = DateTime.UtcNow,
                RateMasterUpdated = DateTime.UtcNow
            };

            _context.RateMasters.Add(rateMaster);
            await _context.SaveChangesAsync();

            return Ok(rateMaster);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // 🔹 UPDATE (COMPANY SAFE)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RateMaster dto)
    {
        var companyId = GetCompanyId();

        var rate = await _context.RateMasters
            .FirstOrDefaultAsync(r =>
                r.RateMasterId == id &&
                r.RateMasterCompanyId == companyId);

        if (rate == null)
            return NotFound();

        rate.RateMasterApplicableDt = dto.RateMasterApplicableDt;
        rate.RateMasterPartyId = dto.RateMasterPartyId;
        rate.RateMasterServiceId = dto.RateMasterServiceId;
        rate.RateMasterSaleRate = dto.RateMasterSaleRate;
        rate.RateMasterPurchaseRate = dto.RateMasterPurchaseRate;

        rate.RateMasterUpdateByUserId = GetUserId();
        rate.RateMasterUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(rate);
    }

    // 🔹 DELETE (COMPANY SAFE)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var companyId = GetCompanyId();

        var rate = await _context.RateMasters
            .FirstOrDefaultAsync(r =>
                r.RateMasterId == id &&
                r.RateMasterCompanyId == companyId);

        if (rate == null)
            return NotFound();

        _context.RateMasters.Remove(rate);
        await _context.SaveChangesAsync();

        return Ok(true);
    }
}
