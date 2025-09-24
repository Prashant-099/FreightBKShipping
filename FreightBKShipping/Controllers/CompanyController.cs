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
        public CompanyController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany(_context.companies.AsNoTracking(), "CompanyId");

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedCompanies = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CompanyDto
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    Code = c.Code,
                    Address1 = c.Address1,
                    Email = c.Email,
                    Mobile = c.Mobile,
                    IsGstApplicable = c.IsGstApplicable,
                    Gstin = c.Gstin,
                    Status = c.Status,
                    Remarks = c.Remarks,
                    City = c.City,
                    Country = c.Country,
                       StateId = c.StateId,
                        Panno = c.Panno
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
    .FirstOrDefaultAsync(c => c.CompanyId == id);
            if (company == null) return NotFound();

            return Ok(new CompanyDto
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                Code = company.Code,
                Address1 = company.Address1,
                Email = company.Email,
                Mobile = company.Mobile,
                IsGstApplicable = company.IsGstApplicable,
                Gstin = company.Gstin,
                Status = company.Status,
                Remarks = company.Remarks,
                City = company.City,
                Country = company.Country,
                Panno = company.Panno
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompanyAddDto dto)
        {

            var company = new Company
            {
                Name = dto.Name,
                Code = dto.Code,
                Address1 = dto.Address1,
                Email = dto.Email,
                Mobile = dto.Mobile,
                IsGstApplicable = dto.IsGstApplicable,
                Gstin = dto.Gstin,
                Status = dto.Status,
                Remarks = dto.Remarks,
                City = dto.City,
                Country = dto.Country,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                StateId = dto.StateId,
                Panno = dto.Panno,
                AddedByUserId = GetUserId().ToString() // 👈 replace with logged-in user if available
            };

            _context.companies.Add(company);
            await _context.SaveChangesAsync();

            return Ok(new { company.CompanyId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CompanyDto dto)
        {
            var company = await _context.companies.FindAsync(id);
            if (company == null) return NotFound();

            company.Name = dto.Name;
            company.Code = dto.Code;
            company.Address1 = dto.Address1;
            company.Email = dto.Email;
            company.Mobile = dto.Mobile;
            company.IsGstApplicable = dto.IsGstApplicable;
            company.Gstin = dto.Gstin;
            company.Status = dto.Status;
            company.Remarks = dto.Remarks;
            company.City = dto.City;
            company.Country = dto.Country;
            company.Updated = DateTime.UtcNow;
            company.StateId = dto.StateId;
            company.Panno = dto.Panno;
            company.UpdatedByUserId = GetUserId().ToString(); // 👈 replace with logged-in user

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
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
