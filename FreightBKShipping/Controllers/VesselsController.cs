using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.DTOs.VesselDto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VesselsController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;

        public VesselsController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;

            _context = context;
        }

        // GET: api/Vessels
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vessels = await FilterByCompany(_context.Vessels, "VesselCompanyId").OrderByDescending(b=>b.VesselId).ToListAsync();
            return Ok(vessels);
        }

        // GET: api/Vessels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vessel = await FilterByCompany(_context.Vessels, "VesselCompanyId")
                .FirstOrDefaultAsync(v => v.VesselId == id);
            if (vessel == null) return NotFound();
            return Ok(vessel);
        }

        // POST: api/Vessels
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VesselCreateDto dto)
        {
            var exists = await _context.Vessels.AnyAsync(v =>
    v.VesselCompanyId == GetCompanyId() &&
    v.VesselName.Replace(" ","").ToLower() == dto.VesselName.Replace(" ", "").ToLower());

            if (exists)
            {
                return BadRequest( $"Vessel Already Exists.");
            }
            var vessel = new Vessel
            {
                VesselUuid = Guid.NewGuid().ToString("N").Substring(0, 30),
                VesselAddedByUserId = GetUserId(),
                VesselUpdatedByUserId = GetUserId(),
                VesselName = dto.VesselName,
                VesselStatus = dto.VesselStatus,
                VesselCompanyId = GetCompanyId(),
                VesselQty = dto.VesselQty,
                VesselCbm = dto.VesselCbm,
                VesselDtSailing = dto.VesselDtSailing,
                VesselDtBerting = dto.VesselDtBerting,
                VesselDtDemmurate = dto.VesselDtDemmurate,
                VesselNoOfBL = dto.VesselNoOfBL,
                VesselQtyOpening = dto.VesselQtyOpening,
                VesselCbmOpening = dto.VesselCbmOpening,
                VesselNoOfBLOpening = dto.VesselNoOfBLOpening,
                VesselsCol = dto.VesselsCol,
                VesselEta = dto.VesselEta,
                VesselEdt = dto.VesselEdt,
                VesselAta = dto.VesselAta,
                VesselCreated = DateTime.UtcNow,
                VesselUpdated = DateTime.UtcNow
            };

            _context.Vessels.Add(vessel);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Vessel",
                RecordId = vessel.VesselId,
                VoucherType = "Vessel",
                Amount = 0,
                Operations = "INSERT",
                Remarks = vessel.VesselName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(vessel);
        }

        // PUT: api/Vessels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VesselUpdateDto dto)
        {
            var vessel = await _context.Vessels.FindAsync(id);
            if (vessel == null) return NotFound();

            var exists = await _context.Vessels.AnyAsync(v =>
    v.VesselCompanyId == GetCompanyId() &&
    v.VesselId != id &&
    v.VesselName.Replace(" ", "").ToLower() == dto.VesselName.Replace(" ", "").ToLower()
);

            if (exists)
            {
                return BadRequest(new { message = "Vessel Already Exists." });
            }

            vessel.VesselUpdatedByUserId = GetUserId();
            vessel.VesselName = dto.VesselName;
            vessel.VesselStatus = dto.VesselStatus;
            vessel.VesselQty = dto.VesselQty;
            vessel.VesselCbm = dto.VesselCbm;
            vessel.VesselDtSailing = dto.VesselDtSailing;
            vessel.VesselDtBerting = dto.VesselDtBerting;
            vessel.VesselDtDemmurate = dto.VesselDtDemmurate;
            vessel.VesselNoOfBL = dto.VesselNoOfBL;
            vessel.VesselQtyOpening = dto.VesselQtyOpening;
            vessel.VesselCbmOpening = dto.VesselCbmOpening;
            vessel.VesselNoOfBLOpening = dto.VesselNoOfBLOpening;
            vessel.VesselsCol = dto.VesselsCol;
            vessel.VesselEta = dto.VesselEta;
            vessel.VesselEdt = dto.VesselEdt;
            vessel.VesselAta = dto.VesselAta;
            vessel.VesselUpdated = DateTime.UtcNow;
            vessel.VesselCompanyId = GetCompanyId();

            _context.Vessels.Update(vessel);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Vessel",
                RecordId = vessel.VesselId,
                VoucherType = "Vessel",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = vessel.VesselName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(vessel);
        }

        // DELETE: api/Vessels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vessel = await FilterByCompany(_context.Vessels, "VesselCompanyId")
                .FirstOrDefaultAsync(v => v.VesselId == id);
            if (vessel == null) return NotFound();
            // 🔎 Check used in Jobs
            bool usedInJobs = await _context.Jobs.AnyAsync(j =>
               j.JobVesselId == id  &&
                j.JobCompanyId == GetCompanyId() &&
                j.JobActive == true
            );

            if (usedInJobs)
            {
                return BadRequest( $"It is used in Jobs.");
            }
            // 🔎 Check used in Bills
            bool usedInBills = await _context.Bills.AnyAsync(b =>
                b.BillVesselId == id  &&
                b.BillCompanyId == GetCompanyId() &&
                b.BillStatus == true
            );

            if (usedInBills)
            {
                return BadRequest( $"It is used in Bills.");
            }
            _context.Vessels.Remove(vessel);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Vessel",
                RecordId = vessel.VesselId,
                VoucherType = "Vessel",
                Amount = 0,
                Operations = "DELETE",
                Remarks = vessel.VesselName,
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(true);
        }
    }
}
