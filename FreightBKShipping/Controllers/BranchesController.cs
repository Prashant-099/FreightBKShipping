using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : BaseController
    {
        private readonly AppDbContext _context;
        public BranchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Branches
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await FilterByCompany( _context.Branches, "BranchCompanyId").OrderByDescending(b => b.BranchId).ToListAsync();
            return Ok(branches);
        }

        // GET: api/Branches/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var branch = await FilterByCompany( _context.Branches, "BranchCompanyId").FirstOrDefaultAsync(b => b.BranchId == id); 
            if (branch == null) return NotFound();
            return Ok(branch);
        }

        // POST: api/Branches
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchCreateDto dto)
        {

            if (dto.Branchisdefault)
            {
                var existingDefaults = await _context.Branches
                    .Where(b => b.BranchCompanyId == GetCompanyId() && b.Branchisdefault)
                    .ToListAsync();

                foreach (var b in existingDefaults)
                {
                    b.Branchisdefault = false;
                }
            }
            var branch = new Branch
            {
                BranchName = dto.BranchName,
                BranchGstin = dto.BranchGstin,
                BranchPan = dto.BranchPan,
                BranchPrintName = dto.BranchPrintName,
                BranchAddress1 = dto.BranchAddress1,
                BranchAddress2 = dto.BranchAddress2,
                BranchAddress3 = dto.BranchAddress3,
                BranchPincode = dto.BranchPincode,
                BranchStateId = dto.BranchStateId,
                BranchCountry = dto.BranchCountry,
                BranchStateCode = dto.BranchStateCode,
                BranchContactNo = dto.BranchContactNo,
                BranchEmail = dto.BranchEmail,
                BranchCity = dto.BranchCity,
                Branchisdefault = dto.Branchisdefault,
                BranchCompanyId = GetCompanyId(),
                BranchAddedBy =GetUserId(),
                BranchStatus = dto.BranchStatus,
                BranchCreated = DateTime.UtcNow,
                BranchUpdated= DateTime.UtcNow,
                BranchUpdatedBy= GetUserId()
                
            };

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return Ok(branch);
        }

        // PUT: api/Branches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BranchUpdateDto dto)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null) return NotFound();

            if (dto.Branchisdefault)
            {
                var existingDefaults = await _context.Branches
                    .Where(b => b.BranchCompanyId == GetCompanyId() && b.Branchisdefault)
                    .ToListAsync();

                foreach (var b in existingDefaults)
                {
                    b.Branchisdefault = false;
                }
            }

            branch.BranchName = dto.BranchName;
            branch.BranchGstin = dto.BranchGstin;
            branch.BranchPan = dto.BranchPan;
            branch.BranchPrintName = dto.BranchPrintName;
            branch.BranchAddress1 = dto.BranchAddress1;
            branch.BranchAddress2 = dto.BranchAddress2;
            branch.BranchAddress3 = dto.BranchAddress3;
            branch.BranchPincode = dto.BranchPincode;
            branch.BranchStateId = dto.BranchStateId;
            branch.BranchCountry = dto.BranchCountry;
            branch.BranchStateCode = dto.BranchStateCode;
            branch.BranchContactNo = dto.BranchContactNo;
            branch.BranchEmail = dto.BranchEmail;
            branch.BranchCity = dto.BranchCity;
            branch.Branchisdefault = dto.Branchisdefault;
            branch.BranchStatus = dto.BranchStatus;
            branch.BranchUpdated = DateTime.UtcNow;
            branch.BranchUpdatedBy = GetUserId();
            branch.BranchCompanyId = GetCompanyId();

            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
            return Ok(branch);
        }

        // DELETE: api/Branches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await FilterByCompany(_context.Branches, "BranchCompanyId").FirstOrDefaultAsync(b => b.BranchId == id); ;
            if (branch == null) return NotFound();

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
