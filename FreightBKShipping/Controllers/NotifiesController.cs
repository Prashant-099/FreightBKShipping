using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifiesController : BaseController
    {
        private readonly AppDbContext _context;

        public NotifiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Notifies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var notifies = await FilterByCompany(_context.Notifies, "NotifyCompanyId").OrderByDescending(b=>b.NotifyId).ToListAsync();
                return Ok(notifies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        // GET: api/Notifies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var notify = await FilterByCompany(_context.Notifies, "NotifyCompanyId")
                                    .FirstOrDefaultAsync(n => n.NotifyId == id);

                if (notify == null) return NotFound();

                return Ok(notify);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        // POST: api/Notifies
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotifyCreateDto dto)
        {
            try
            {
                // Check duplicate
                var exists = await _context.Notifies
                    .AnyAsync(n => n.NotifyCompanyId == GetCompanyId() &&
                                   n.NotifyType == dto.NotifyType &&
                                   n.NotifyName == dto.NotifyName);

                if (exists)
                    return BadRequest("A notify with the same name and type already exists.");

                string? stateName = " ";
                string? stateCode = " ";

                if (dto.NotifyStateId > 0)
                {
                    var state = await _context.States.FindAsync(dto.NotifyStateId);
                    if (state != null)
                    {
                        stateName = state.StateName;
                        stateCode = state.StateCode;
                    }
                }

                var notify = new Notify
                {
                    NotifyName = dto.NotifyName,
                    NotifyType = dto.NotifyType,
                    NotifyAddress1 = dto.NotifyAddress1,
                    NotifyCity = dto.NotifyCity,
                    NotifyStateId = dto.NotifyStateId,
                    NotifyState = stateName,
                    NotifyStateCode = stateCode,
                    NotifyPincode = dto.NotifyPincode,
                    NotifyCountry = dto.NotifyCountry,
               
                    NotifyEmail = dto.NotifyEmail,
                    NotifyContactNo = dto.NotifyContactNo,
                    NotifyGstNo = dto.NotifyGstNo,
                    NotifyPan = dto.NotifyPan,
                    NotifyStatus = dto.NotifyStatus,
                    
                    NotifyCompanyId = GetCompanyId(),
                    NotifyAddedByUserId = GetUserId(),
                    NotifyUpdatedByUserId = GetUserId(),
                    NotifyCreated = DateTime.UtcNow,
                    NotifyUpdated = DateTime.UtcNow
                };

                _context.Notifies.Add(notify);
                await _context.SaveChangesAsync();
                return Ok(notify);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        // PUT: api/Notifies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NotifyUpdateDto dto)
        {
            try
            {
                var notify = await _context.Notifies.FindAsync(id);
                if (notify == null) return NotFound();

                // Check duplicate for other records
                var exists = await _context.Notifies
                    .AnyAsync(n => n.NotifyCompanyId == GetCompanyId() &&
                                   n.NotifyType == dto.NotifyType &&
                                   n.NotifyName == dto.NotifyName &&
                                   n.NotifyId != id);

                if (exists)
                    return BadRequest("A notify with the same name and type already exists.");
                string? stateName = " ";
                string? stateCode = " ";
                var state = await _context.States.FindAsync(dto.NotifyStateId);
                if (state != null)
                {
                    stateName = state.StateName;
                    stateCode = state.StateCode;

                }
                //if (state == null) return BadRequest("Invalid StateId");

                notify.NotifyName = dto.NotifyName;
                notify.NotifyType = dto.NotifyType;
                notify.NotifyAddress1 = dto.NotifyAddress1;
                notify.NotifyCity = dto.NotifyCity;
                notify.NotifyStateId = dto.NotifyStateId;
                notify.NotifyState = stateName;
                notify.NotifyStateCode = stateCode;
                notify.NotifyPincode = dto.NotifyPincode;
                notify.NotifyCountry = dto.NotifyCountry;
             

                notify.NotifyEmail = dto.NotifyEmail;
                notify.NotifyContactNo = dto.NotifyContactNo;
                notify.NotifyGstNo = dto.NotifyGstNo;
                notify.NotifyPan = dto.NotifyPan;
                notify.NotifyStatus = dto.NotifyStatus;
                notify.NotifyUpdated = DateTime.UtcNow;
                notify.NotifyUpdatedByUserId = GetUserId();
                notify.NotifyCompanyId = GetCompanyId();

                _context.Notifies.Update(notify);
                await _context.SaveChangesAsync();

                return Ok(notify);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        // DELETE: api/Notifies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var notify = await FilterByCompany(_context.Notifies, "NotifyCompanyId")
                                    .FirstOrDefaultAsync(n => n.NotifyId == id);

                if (notify == null) return NotFound();

                _context.Notifies.Remove(notify);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }
    }
}
