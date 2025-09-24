using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.PayTypeDto;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayTypeController : BaseController
    {
        private readonly AppDbContext _context;

        public PayTypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PayType
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var paytypes = await FilterByCompany(_context.PayTypes, "PayTypeCompanyId").ToListAsync();

            var result = paytypes.Select(p => new PayTypeReadDto
            {
                PayTypeId = p.PayTypeId,
                PayTypeName = p.PayTypeName,
                PayTypeStatus = p.PayTypeStatus
            });

            return Ok(result);
        }

        // GET: api/PayType/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await FilterByCompany(_context.PayTypes, "PayTypeCompanyId")
                                .FirstOrDefaultAsync(p => p.PayTypeId == id);

            if (p == null) return NotFound("PayType not found");

            var dto = new PayTypeReadDto
            {
                PayTypeId = p.PayTypeId,
                PayTypeName = p.PayTypeName,
                PayTypeStatus = p.PayTypeStatus
            };

            return Ok(dto);
        }

        // POST: api/PayType
        [HttpPost]
        public async Task<IActionResult> Create(PayTypeCreateDto dto)
        {
            var paytype = new PayType
            {
                PayTypeName = dto.PayTypeName,
                PayTypeStatus = dto.PayTypeStatus,
                PayTypeCompanyId = GetCompanyId(),
                PayTypeAddedByUserId = GetUserId(),
                PayTypeCreated = DateTime.UtcNow,
                PayTypeUpdated = DateTime.UtcNow
            };

            _context.PayTypes.Add(paytype);
            await _context.SaveChangesAsync();

            return Ok(paytype.PayTypeId);
        }

        // PUT: api/PayType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PayTypeCreateDto dto)
        {
            var paytype = await FilterByCompany(_context.PayTypes, "PayTypeCompanyId")
                                .FirstOrDefaultAsync(p => p.PayTypeId == id);

            if (paytype == null) return NotFound("PayType not found");

            paytype.PayTypeName = dto.PayTypeName;
            paytype.PayTypeStatus = dto.PayTypeStatus;
            paytype.PayTypeUpdatedByUserId = GetUserId();
            paytype.PayTypeUpdated = DateTime.UtcNow;
            paytype.PayTypeCompanyId = GetCompanyId();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/PayType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var paytype = await FilterByCompany(_context.PayTypes, "PayTypeCompanyId")
                                .FirstOrDefaultAsync(p => p.PayTypeId == id);

            if (paytype == null) return NotFound("PayType not found");

            _context.PayTypes.Remove(paytype);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
