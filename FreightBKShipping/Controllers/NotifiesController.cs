using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
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
        private readonly AuditLogService _auditLogService;

        public NotifiesController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;

            _context = context;
        }

        // GET: api/Notifies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
                var notifies = await FilterByCompany(_context.Notifies, "NotifyCompanyId").OrderByDescending(b=>b.NotifyId).ToListAsync();
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
                                   n.NotifyType.ToLower() == dto.NotifyType.ToLower() &&
                                   n.NotifyName.Replace(" ","").ToLower() == dto.NotifyName.Replace(" ", "").ToLower());

                if (exists)
                    return BadRequest( $"It Already Exists in '{dto.NotifyType}'.");

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
                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "Miscellaneous",
                    RecordId = notify.NotifyId,
                    VoucherType = "Miscellaneous",
                    Amount = 0,
                    Operations = "INSERT",
                    Remarks = notify.NotifyName,
                    BranchId = 0,
                    YearId = 0
                }, GetCompanyId());
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
                                   n.NotifyType.ToLower() == dto.NotifyType.ToLower() &&
                                   n.NotifyName.Replace(" ", "").ToLower() == dto.NotifyName.Replace(" ", "").ToLower() &&
                                   n.NotifyId != id);

                if (exists)
                    return BadRequest(new { Message = $"It Already Exists in '{dto.NotifyType}'." });
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
                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "Miscellaneous",
                    RecordId = notify.NotifyId,
                    VoucherType = "Miscellaneous",
                    Amount = 0,
                    Operations = "UPDATE",
                    Remarks = notify.NotifyName,
                    BranchId = 0,
                    YearId = 0
                }, GetCompanyId());
                return Ok(notify);
            
        }

        // DELETE: api/Notifies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
                var notify = await FilterByCompany(_context.Notifies, "NotifyCompanyId")
                                    .FirstOrDefaultAsync(n => n.NotifyId == id);

                if (notify == null) return NotFound();

            // 🔎 Check used in Jobs
            bool usedInJobs = await _context.Jobs.AnyAsync(j =>
               (j.JobLineId == id || j.JobConsigneeId == id || j.JobShipperId == id || j.JobSupplierId == id || j.JobChaId == id) &&
                j.JobCompanyId == GetCompanyId() &&
                j.JobActive == true
            );

            if (usedInJobs)
            {
                return BadRequest( $"It is used in Jobs.");
            }
            // 🔎 Check used in Bills
            bool usedInBills = await _context.Bills.AnyAsync(b =>
                (b.BillShipperId == id || b.BillConsigneeId == id) &&
                b.BillCompanyId == GetCompanyId() &&
                b.BillStatus == true
            );

            if (usedInBills)
            {
                return BadRequest($"It is used in Bills.");
            }

            _context.Notifies.Remove(notify);
                await _context.SaveChangesAsync();
                await _auditLogService.AddAsync(new AuditLogCreateDto
                {
                    TableName = "Miscellaneous",
                    RecordId = id,
                    VoucherType = "Miscellaneous",
                    Amount = 0,
                    Operations = "DELETE",
                    Remarks = notify.NotifyName,
                    BranchId = 0,
                    YearId = 0
                }, GetCompanyId());
                return Ok(true);
            
        }
    }
}
