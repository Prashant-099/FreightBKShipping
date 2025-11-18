using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var notifies = await FilterByCompany(_context.Notifies, "NotifyCompanyId").ToListAsync();
            return Ok(notifies);
        }

        // GET: api/Notifies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var notify = await FilterByCompany(_context.Notifies, "NotifyCompanyId")
                                .FirstOrDefaultAsync(n => n.NotifyId == id);
            if (notify == null) return NotFound();
            return Ok(notify);
        }

        // POST: api/Notifies
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotifyCreateDto dto)
        {
            // Check duplicate
            var exists = await _context.Notifies
                .AnyAsync(n => n.NotifyCompanyId == GetCompanyId() &&
                               n.NotifyType == dto.NotifyType &&
                               n.NotifyName == dto.NotifyName);
            if (exists)
                return BadRequest("A notify with the same name and type already exists.");
            // Fetch state name from States table
            //  var state = await _context.States.FindAsync(dto.NotifyStateId);
            // if (state == null) return BadRequest("Invalid StateId");
            string? stateName = " ";
            string? stateCode = " ";

            if ( dto.NotifyStateId > 0)
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
                //NotifyAddress2 = dto.NotifyAddress2,
                //NotifyAddress3 = dto.NotifyAddress3,
                NotifyCity = dto.NotifyCity,
                NotifyStateId = dto.NotifyStateId,
                NotifyState = stateName, // automatic join
                NotifyStateCode = stateCode,
                NotifyPincode = dto.NotifyPincode,
                NotifyCountry = dto.NotifyCountry,
                NotifyTel = dto.NotifyContactNo,
                NotifyEmail = dto.NotifyEmail,
                NotifyContactNo = dto.NotifyContactNo,
                //NotifyContactPerson = dto.NotifyContactPerson,
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


        // PUT: api/Notifies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NotifyUpdateDto dto)
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
            var state = await _context.States.FindAsync(dto.NotifyStateId);
            if (state == null) return BadRequest("Invalid StateId");

            notify.NotifyName = dto.NotifyName;
            notify.NotifyType = dto.NotifyType;
            notify.NotifyAddress1 = dto.NotifyAddress1;
            //notify.NotifyAddress2 = dto.NotifyAddress2;
            //notify.NotifyAddress3 = dto.NotifyAddress3;
            notify.NotifyCity = dto.NotifyCity;
            notify.NotifyStateId = dto.NotifyStateId;
            notify.NotifyState = state.StateName; // automatic join
            notify.NotifyStateCode = state.StateCode;
            notify.NotifyPincode = dto.NotifyPincode;
            notify.NotifyCountry = dto.NotifyCountry;
            notify.NotifyTel = dto.NotifyContactNo;
            notify.NotifyEmail = dto.NotifyEmail;
            notify.NotifyContactNo = dto.NotifyContactNo;
            //notify.NotifyContactPerson = dto.NotifyContactPerson;
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


        // DELETE: api/Notifies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var notify = await FilterByCompany(_context.Notifies, "NotifyCompanyId")
                                .FirstOrDefaultAsync(n => n.NotifyId == id);
            if (notify == null) return NotFound();

            _context.Notifies.Remove(notify);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
