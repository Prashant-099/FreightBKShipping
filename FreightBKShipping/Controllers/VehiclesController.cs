using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : BaseController
    {
        private readonly AppDbContext _context;

        public VehiclesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await FilterByCompany(_context.Vehicles, "VehicleCompanyId")
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
                .AnyAsync(v => v.VehicleNo == dto.VehicleNo && v.VehicleCompanyId == GetCompanyId());

            if (exists)
                return BadRequest(new { message = "Vehicle number already exists." });

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

            return Ok(vehicle);
        }

        // PUT: api/Vehicles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleCreateDto dto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

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

            return Ok(vehicle);
        }

        // DELETE (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

            vehicle.VehicleStatus = 2; // soft delete
            vehicle.VehicleUpdated = DateTime.UtcNow;
            vehicle.VehicleUpdatedByUserId = GetUserId();

            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}