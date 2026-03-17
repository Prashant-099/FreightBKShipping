using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Auditlogdto;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly AuditLogService _auditLogService;  
        public VehiclesController(AppDbContext context, AuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await FilterByCompany(_context.Vehicles, "VehicleCompanyId").Where(v=>v.VehicleStatus==1)
                .OrderByDescending(v => v.VehicleId)
                .ToListAsync();

            return Ok(vehicles);
        }

        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var vehicle = await FilterByCompany(_context.Vehicles, "VehicleCompanyId")
                .FirstOrDefaultAsync(v => v.VehicleId == id);

            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        // POST: api/Vehicles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleCreateDto dto)
        {
            // Unique vehicle_no check
            var exists = await _context.Vehicles
                .AnyAsync(v => v.VehicleNo.Replace(" ","").ToLower() == dto.VehicleNo.Replace(" ", "").ToLower() && v.VehicleCompanyId == GetCompanyId() && v.VehicleStatus==1);

            if (exists)
                return BadRequest( "Vehicle number already exists." );

            var vehicle = new Vehicle
            {
                VehicleCompanyId = GetCompanyId(),
                VehicleAddedByUserId = GetUserId(),
                VehicleNo = dto.VehicleNo,
                VehicleOwnerType = dto.VehicleOwnerType,
                VehicleAccountId = dto.VehicleAccountId,
                VehicleTypeId = dto.VehicleTypeId,
                VehicleGroupId = dto.VehicleGroupId,
                VehicleAverage = dto.VehicleAverage,
                VehicleRTO = dto.VehicleRTO,
                VehicleEngineNo = dto.VehicleEngineNo,
                VehicleChassisNo = dto.VehicleChassisNo,
                VehicleLoadCapacity = dto.VehicleLoadCapacity,
                VehicleMake = dto.VehicleMake,
                VehicleModel = dto.VehicleModel,
                VehicleRemarks = dto.VehicleRemarks,
                VehicleStatus = dto.VehicleStatus,
                VehicleFastage = dto.VehicleFastage,
                VehicleGpsNo = dto.VehicleGpsno,

                VehicleTax = dto.VehicleTax,
                VehicleFitness = dto.VehicleFitness,
                VehicleStatePermit = dto.VehicleStatepermit,
                VehicleInsurance = dto.VehicleInsurance,
                VehiclePUC = dto.VehiclePuc,
                VehicleForm9 = dto.VehicleForm9,
                VehicleCalibration = dto.VehicleCalibration,
                VehicleEmi = dto.VehicleEmi,

                VehicleDriverId = dto.VehicleDriverId,
                VehicleCreated = DateTime.UtcNow,
                VehicleUpdated = DateTime.UtcNow
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Vehicle",
                RecordId = vehicle.VehicleId,
                VoucherType = "Vehicle",
                Amount = 0,
                Operations = "INSERT",
                Remarks = $"{vehicle.VehicleNo}",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(vehicle);
        }

        // PUT: api/Vehicles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleCreateDto dto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();
            // Unique vehicle_no check
            var exists = await _context.Vehicles
                .AnyAsync(v => v.VehicleNo.Replace(" ", "").ToLower() == dto.VehicleNo.Replace(" ", "").ToLower() && v.VehicleId != id && v.VehicleCompanyId == GetCompanyId() && v.VehicleStatus == 1);

            if (exists)
                return BadRequest(new { message = "Vehicle number already exists." });

            vehicle.VehicleUpdatedByUserId = GetUserId();
            vehicle.VehicleUpdated = DateTime.UtcNow;

            vehicle.VehicleOwnerType = dto.VehicleOwnerType;
            vehicle.VehicleAccountId = dto.VehicleAccountId;
            vehicle.VehicleTypeId = dto.VehicleTypeId;
            vehicle.VehicleGroupId = dto.VehicleGroupId;
            vehicle.VehicleAverage = dto.VehicleAverage;
            vehicle.VehicleRTO = dto.VehicleRTO;
            vehicle.VehicleEngineNo = dto.VehicleEngineNo;
            vehicle.VehicleChassisNo = dto.VehicleChassisNo;
            vehicle.VehicleLoadCapacity = dto.VehicleLoadCapacity;
            vehicle.VehicleMake = dto.VehicleMake;
            vehicle.VehicleModel = dto.VehicleModel;
            vehicle.VehicleRemarks = dto.VehicleRemarks;
            vehicle.VehicleStatus = dto.VehicleStatus;
            vehicle.VehicleFastage = dto.VehicleFastage;
            vehicle.VehicleGpsNo = dto.VehicleGpsno;

            vehicle.VehicleTax = dto.VehicleTax;
            vehicle.VehicleFitness = dto.VehicleFitness;
            vehicle.VehicleStatePermit = dto.VehicleStatepermit;
            vehicle.VehicleNationalPermit = dto.VehicleNational;
            vehicle.VehicleInsurance = dto.VehicleInsurance;
            vehicle.VehiclePUC = dto.VehiclePuc;
            vehicle.VehicleForm9 = dto.VehicleForm9;
            vehicle.VehicleCalibration = dto.VehicleCalibration;
            vehicle.VehicleEmi = dto.VehicleEmi;

            vehicle.VehicleDriverId = dto.VehicleDriverId;

            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Vehicle",
                RecordId = vehicle.VehicleId,
                VoucherType = "Vehicle",
                Amount = 0,
                Operations = "UPDATE",
                Remarks = $"{vehicle.VehicleNo}",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(vehicle);
        }

        // DELETE (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();
            var lr = await _context.Lrs
        .Where(j => j.LrVehicleId == id && j.LrCompanyId == GetCompanyId() && j.LrStatus == 1)
        .Select(j => new { j.LrNoStr })
        .FirstOrDefaultAsync();

            if (lr != null)
            {
                return BadRequest( $"This Vehicle Number is used in LR No. '{lr.LrNoStr}'.");
            }
            vehicle.VehicleStatus = 2; // soft delete
            vehicle.VehicleUpdated = DateTime.UtcNow;
            vehicle.VehicleUpdatedByUserId = GetUserId();

            await _context.SaveChangesAsync();
            await _auditLogService.AddAsync(new AuditLogCreateDto
            {
                TableName = "Vehicle",
                RecordId = id,
                VoucherType = "Vehicle",
                Amount = 0,
                Operations = "DELETE",
                Remarks = $"{vehicle.VehicleNo}",
                BranchId = 0,
                YearId = 0
            }, GetCompanyId());
            return Ok(true);
        }
    }
}