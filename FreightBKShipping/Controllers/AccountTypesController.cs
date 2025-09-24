using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypesController : BaseController
    {
        private readonly AppDbContext _context;

        public AccountTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await FilterByCompany( _context.AccountTypes, "CompanyId").ToListAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var type = await FilterByCompany( _context.AccountTypes, "CompanyId").FirstOrDefaultAsync(b => b.AccountTypeId == id); 
            if (type == null) return NotFound();
            return Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType model)
        {
            model.CompanyId = GetCompanyId();
            model.AddedByUserId = GetUserId();
            model.UpdatedByUserId = GetUserId();
            model.AccountTypeUpdated = DateTime.UtcNow;
            model.AccountTypeCreated = DateTime.UtcNow;
            _context.AccountTypes.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AccountType model)
        {
            if (id != model.AccountTypeId) return BadRequest();
            model.CompanyId = GetCompanyId();
            model.UpdatedByUserId = GetUserId();
            model.AccountTypeUpdated = DateTime.UtcNow;
         
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await FilterByCompany(_context.AccountTypes, "CompanyId").FirstOrDefaultAsync(b => b.AccountTypeId == id);
            if (type == null) return NotFound();

            _context.AccountTypes.Remove(type);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
