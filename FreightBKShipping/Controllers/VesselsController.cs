using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.VesselDto;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VesselsController : BaseController
    {
        private readonly AppDbContext _context;

        public VesselsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vessels
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vessels = await FilterByCompany(_context.Vessels, "VesselCompanyId").ToListAsync();
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
            return Ok(vessel);
        }

        // PUT: api/Vessels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VesselUpdateDto dto)
        {
            var vessel = await _context.Vessels.FindAsync(id);
            if (vessel == null) return NotFound();

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
            return Ok(vessel);
        }

        // DELETE: api/Vessels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vessel = await FilterByCompany(_context.Vessels, "VesselCompanyId")
                .FirstOrDefaultAsync(v => v.VesselId == id);
            if (vessel == null) return NotFound();

            _context.Vessels.Remove(vessel);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
