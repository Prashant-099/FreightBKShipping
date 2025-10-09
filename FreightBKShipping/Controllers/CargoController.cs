﻿using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.DTOs.Cargo;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : BaseController 
    {
        private readonly AppDbContext _context;
        public CargoController(AppDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CargoCreateDto dto)
        {
            // Fetch selected HSN record
            var hsn = await _context.HsnSacs.FirstOrDefaultAsync(h => h.HsnId == dto.CargoHsn);

            var cargo = new Cargo
            {
                CargoCompanyId = GetCompanyId(),
                CargoAddbyUserId = GetUserId(),
                CargoUpdatedbyUserId= GetUserId(),
                CargoName = dto.CargoName,
                CargoType = dto.CargoType,
                CargoRemarks = dto.CargoRemarks,
                CargoHsn = dto.CargoHsn,
                CargoGstPer = hsn?.HsnGstPer ?? 0,
                CargoCess = hsn?.HsnCess ?? 0,
                CargoStatus = dto.CargoStatus,
                CargoCreated = DateTime.UtcNow,
                CargoUpdated = DateTime.UtcNow
            };

            _context.Cargoes.Add(cargo);
            await _context.SaveChangesAsync();
            return Ok(cargo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CargoUpdateDto dto)
        {
            var cargo = await _context.Cargoes.FindAsync(id);
            if (cargo == null) return NotFound();
            // Fetch new HSN record
            var hsn = await _context.HsnSacs.FirstOrDefaultAsync(h => h.HsnId == dto.CargoHsn);

            cargo.CargoName = dto.CargoName;
            cargo.CargoType = dto.CargoType;
            cargo.CargoRemarks = dto.CargoRemarks;
            cargo.CargoHsn = dto.CargoHsn;
            cargo.CargoGstPer = hsn?.HsnGstPer ?? 0;
            cargo.CargoCess = hsn?.HsnCess ?? 0;
            cargo.CargoStatus = dto.CargoStatus;
            cargo.CargoUpdated = DateTime.UtcNow;
            cargo.CargoCompanyId = GetCompanyId();
            cargo.CargoUpdatedbyUserId = GetUserId();

            await _context.SaveChangesAsync();
            return Ok(cargo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cargo = await FilterByCompany(_context.Cargoes, "CargoCompanyId").FirstOrDefaultAsync(b => b.CargoId == id);
            if (cargo == null) return NotFound();

            _context.Cargoes.Remove(cargo);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cargoes = await FilterByCompany( _context.Cargoes, "CargoCompanyId").ToListAsync();
            return Ok(cargoes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cargo = await FilterByCompany( _context.Cargoes, "CargoCompanyId").FirstOrDefaultAsync(b => b.CargoId == id);
            if (cargo == null) return NotFound();
            return Ok(cargo);
        }
    }
}
