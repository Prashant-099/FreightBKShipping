using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerAddController : BaseController
    {
        private readonly AppDbContext _context;

        public ContainerAddController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: api/Lrs
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = FilterByCompany(_context.Lrs, "LrCompanyId")
    .Where(l => l.LrStatus == 1)
    .OrderByDescending(l => l.LrId);


            return Ok(await query.ToListAsync());
        }

        // =========================
        // GET: api/Lrs/{id}
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lr = await FilterByCompany(_context.Lrs, "LrCompanyId")
               .FirstOrDefaultAsync(l => l.LrId == id && l.LrStatus == 1);


            if (lr == null)
                return NotFound();

            return Ok(lr);
        }
        //==================================
        [HttpGet("job/{jobId}")]
        public async Task<IActionResult> GetByJobId(int jobId)
        {
            var lrs = await FilterByCompany(_context.Lrs, "LrCompanyId")
              .Where(l => l.LrJobId == jobId && l.LrStatus == 1)
                .ToListAsync();

            if (lrs == null || !lrs.Any())
                return Ok(new List<Lr>());

            return Ok(lrs); // ✅ List<Lr>
        }

        // =========================
        // POST: api/Lrs
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lr dto)
        {
            try
            {
                dto.LrCompanyId = GetCompanyId();
                dto.LrAddedByUserId = GetUserId();
                dto.LrUpdatedByUserId = GetUserId();
                dto.LrCreated = DateTime.UtcNow;
                dto.LrUpdated = DateTime.UtcNow;
                dto.LrStatus = 1;

                _context.Lrs.Add(dto);
                await _context.SaveChangesAsync();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message,
                    stack = ex.StackTrace
                });
            }
            }

        // =========================
        // PUT: api/Lrs/{id}
        // =========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Lr dto)
        {
            var lr = await _context.Lrs.FindAsync(id);
            if (lr == null)
                return NotFound();

            // 🔹 Copy updatable fields
            lr.LrVoucherId = dto.LrVoucherId;
            lr.LrPartyAccountId = dto.LrPartyAccountId;
            lr.LrConsigneeNotifyId = dto.LrConsigneeNotifyId;
            lr.LrConsignorNotifyId = dto.LrConsignorNotifyId;
            lr.LrProductId = dto.LrProductId;
            lr.LrVehicleId = dto.LrVehicleId;
            lr.LrSupplierAccountId = dto.LrSupplierAccountId;
            lr.LrDriverId = dto.LrDriverId;

            lr.LrNo = dto.LrNo;
            lr.LrNoStr = dto.LrNoStr;
            lr.LrDate = dto.LrDate;
            lr.LrTime = dto.LrTime;
            lr.LrTripNo = dto.LrTripNo;

            lr.LrFromLocationId = dto.LrFromLocationId;
            lr.LrToLocationId = dto.LrToLocationId;
            lr.LrBackLocationId = dto.LrBackLocationId;

            lr.LrContainer1 = dto.LrContainer1;
            lr.LrContainer2 = dto.LrContainer2;

            lr.LrGrossWt = dto.LrGrossWt;
            lr.LrTareWt = dto.LrTareWt;
            lr.LrLoadWt = dto.LrLoadWt;
            lr.LrChargeQty = dto.LrChargeQty;
            lr.LrUnloadWt = dto.LrUnloadWt;

            lr.LrShortWt = dto.LrShortWt;
            lr.LrShortAllowBill = dto.LrShortAllowBill;
            lr.LrShortPerBill = dto.LrShortPerBill;
            lr.LrShortPerTruck = dto.LrShortPerTruck;
            lr.LrShortAllowType = dto.LrShortAllowType;
            lr.LrShortNetBill = dto.LrShortNetBill;

            lr.LrPaymentType = dto.LrPaymentType;
            lr.LrRateBill = dto.LrRateBill;
            lr.LrGrossFreightBill = dto.LrGrossFreightBill;
            lr.LrTripChargeBill = dto.LrTripChargeBill;
            lr.LrAdvanceBill = dto.LrAdvanceBill;
            lr.LrNetFreightBill = dto.LrNetFreightBill;

            lr.LrRefBy = dto.LrRefBy;
            lr.LrStartKm = dto.LrStartKm;
            lr.LrEndKm = dto.LrEndKm;

            lr.LrCustom1 = dto.LrCustom1;
            lr.LrCustom2 = dto.LrCustom2;
            lr.LrCustom3 = dto.LrCustom3;
            lr.LrCustom4 = dto.LrCustom4;
            lr.LrRemarks = dto.LrRemarks;

            // 🔹 NT fields
            lr.LrNtSize = dto.LrNtSize;
            lr.LrNtSealNo = dto.LrNtSealNo;
            lr.LrNtDate = dto.LrNtDate;
            lr.LrNtRfidSeal = dto.LrNtRfidSeal;
            lr.LrNtQty = dto.LrNtQty;
            lr.LrNtInvNo = dto.LrNtInvNo;
            lr.LrNtPickupDt = dto.LrNtPickupDt;
            lr.LrNtPickupLoc = dto.LrNtPickupLoc;
            lr.LrNtShipingBillNo = dto.LrNtShipingBillNo;
            lr.LrNtUnit = dto.LrNtUnit;
            lr.LrNtNetWt = dto.LrNtNetWt;
            lr.LrNtCbm = dto.LrNtCbm;
            lr.LrNtVehicleNo = dto.LrNtVehicleNo;
            lr.LrNtTransporter = dto.LrNtTransporter;
            lr.LrNtGateOutDt = dto.LrNtGateOutDt;
            lr.LrNtGateInDt = dto.LrNtGateInDt;
            lr.LrNtDischargeDt = dto.LrNtDischargeDt;

            // 🔹 Notify fields
            lr.LrConsignorNotifyAddress = dto.LrConsignorNotifyAddress;
            lr.LrConsigneeNotifyAddress = dto.LrConsigneeNotifyAddress;
            lr.LrConsignorNotifyGst = dto.LrConsignorNotifyGst;
            lr.LrConsignorNotifyState = dto.LrConsignorNotifyState;
            lr.LrConsigneeNotifyGst = dto.LrConsigneeNotifyGst;
            lr.LrConsigneeNotifyState = dto.LrConsigneeNotifyState;

            // 🔹 Metadata
            lr.LrUpdatedByUserId = GetUserId();
            lr.LrUpdated = DateTime.UtcNow;
            lr.LrCompanyId = GetCompanyId();

            _context.Lrs.Update(lr);
            await _context.SaveChangesAsync();

            return Ok(lr);
        }

        // =========================
        // DELETE: api/Lrs/{id}
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lr = await FilterByCompany(_context.Lrs, "LrCompanyId")
                .FirstOrDefaultAsync(l => l.LrId == id);

            if (lr == null)
                return NotFound();

            // Soft delete (recommended)
            lr.LrStatus = 2;
            lr.LrUpdatedByUserId = GetUserId();
            lr.LrUpdated = DateTime.UtcNow;

            _context.Lrs.Update(lr);
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
