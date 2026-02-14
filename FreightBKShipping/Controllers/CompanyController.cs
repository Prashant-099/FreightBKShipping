using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace FreightBKShipping.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly CompanySetupService _setupService;

        public CompanyController(AppDbContext context, ISieveProcessor sieveProcessor, CompanySetupService setupService)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
            _setupService = setupService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany(_context.companies.AsNoTracking(), "CompanyId").OrderByDescending(b => b.CompanyId);

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedCompanies = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Join(_context.States,  
            company => company.StateId,
            state => state.StateId,
            (company, state) => new { company, state })
            .Select(c => new CompanyDto
            {
                CompanyId = c.company.CompanyId,
                Name = c.company.Name,
                Code = c.company.Code,

                Address1 = c.company.Address1,
                Address2 = c.company.Address2,
                Address3 = c.company.Address3,
                CompanyAddress = c.company.Address1+ c.company.Address2+ c.company.Address3,

                PrintName = c.company.PrintName,
                Email = c.company.Email,
                Mobile = c.company.Mobile,

                IsGstApplicable = c.company.IsGstApplicable,
                Gstin = c.company.Gstin,
                Status = c.company.Status,
                Remarks = c.company.Remarks,

                City = c.company.City,
                Country = c.company.Country,

                StateId = c.company.StateId,
                StateName = c.state.StateName,
                StateCode = c.state.StateCode,   // agar hai

                Panno = c.company.Panno,
                Website = c.company.Website,
                Pincode = c.company.Pincode,

                CurrencySymbol = c.company.CurrencySymbol,
                Tagline1 = c.company.Tagline1,
                ExtendDays = c.company.ExtendDays,

                HasWhatsapp = c.company.HasWhatsapp,
                FssExpiry =c.company.FssExpiry,
                ContactPerson = c.company.ContactPerson,
                AddedByUserId = c.company.AddedByUserId
            })
            .ToListAsync();


            return Ok(new
            {
                pagination = new
                {
                    page = currentPage,
                    pageSize = pageSize,
                    totalRecords = totalRecords,
                    totalPages = totalPages
                },
                data = pagedCompanies
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> Get(int id)
        {
            var company = await FilterByCompany(_context.companies.AsNoTracking(), "CompanyId")
         .Join(_context.States,  // ✅ Join with states table
             c => c.StateId,
             s => s.StateId,
             (c, s) => new { c, s })
         .Where(cs => cs.c.CompanyId == id)
         .Select(cs => new CompanyDto
         {
             CompanyId = cs.c.CompanyId,
             Name = cs.c.Name,
             Code = cs.c.Code,
             Address1 = cs.c.Address1,
             PrintName = cs.c.PrintName,
             Email = cs.c.Email,
             Mobile = cs.c.Mobile,
             IsGstApplicable = cs.c.IsGstApplicable,
             Gstin = cs.c.Gstin,
             Status = cs.c.Status,
             Remarks = cs.c.Remarks,
             City = cs.c.City,
             Country = cs.c.Country,
             StateId = cs.c.StateId,
             StateName = cs.s.StateName,
             FssExpiry = cs.c.FssExpiry,
             Panno = cs.c.Panno,
             Website= cs.c.Website,
             ExtendDays =cs.c.ExtendDays ?? 0,
         })
         .FirstOrDefaultAsync();

            if (company == null) return NotFound();
            return Ok(company);
        }


        //[HttpPost]
        //[Authorize(Policy = "CompanyManagementOnly")]
        //public async Task<IActionResult> Create(CompanyAddDto dto)
        //{

        //    var company = new Company
        //    {
        //        Name = dto.Name,
        //        Code = dto.Code,
        //        Address1 = dto.Address1,
        //        PrintName= dto.PrintName,
        //        Email = dto.Email,
        //        Mobile = dto.Mobile,
        //        IsGstApplicable = dto.IsGstApplicable,
        //        Gstin = dto.Gstin,
        //        Status = dto.Status,
        //        Remarks = dto.Remarks,
        //        City = dto.City,
        //        Country = dto.Country,
        //        Created = DateTime.UtcNow,
        //        Updated = DateTime.UtcNow,
        //        StateId = dto.StateId,
        //        Panno = dto.Panno,
        //        AddedByUserId = GetUserId().ToString(), // 👈 replace with logged-in user if available
        //        Website=dto.Website,
        //    };

        //    _context.companies.Add(company);
        //    await _context.SaveChangesAsync();



        //    return Ok(new { company.CompanyId });
        //}

        [HttpPost]
        [Authorize(Policy = "CompanyManagementOnly")]
        public async Task<IActionResult> Create(CompanyAddDto dto)
        {
            var userId = GetUserId(); // from token
            var company = await _setupService.CreateCompanyWithDefaultsAsync(dto, userId);
            return Ok(new { company.CompanyId });
        }

        [HttpPut("{id}/admin")]
        [Authorize(Policy = "CompanyManagementOnly")]
        public async Task<IActionResult> UpdateBySuperAdmin(int id, CompanyUpdateDto dto)
        {
            if (id != dto.CompanyId)
                return BadRequest("Company ID mismatch.");

            var userId = GetUserId();
                
            var result = await _setupService
                .UpdateCompanyBySuperAdminAsync(dto, userId);

            if (result == null)
                return NotFound();

            return Ok(new { result.CompanyId });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByCompanyUser(int id, CompanyUpdateDto dto)
        {
            if (id != dto.CompanyId)
                return BadRequest("Company ID mismatch.");

            var userId = GetUserId();
            var userCompanyId = GetCompanyId(); // from token

            var result = await _setupService
                .UpdateCompanyByUserAsync(dto, userId, userCompanyId);

            if (result == null)
                return Forbid();

            return Ok(new { result.CompanyId });
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "CompanyManagementOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _context.companies.FindAsync(id);
            if (company == null) return NotFound();

            _context.companies.Remove(company);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
