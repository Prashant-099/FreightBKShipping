using FreightBKShipping.Data;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountGroupsController : BaseController
    {
        private readonly AppDbContext _context;

        public AccountGroupsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _context.AccountGroups.ToListAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group = await _context.AccountGroups.FindAsync(id);
            if (group == null) return NotFound();
            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountGroup model)
        {
            _context.AccountGroups.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AccountGroup model)
        {
            if (id != model.AccountGroupId) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.AccountGroups.FindAsync(id);
            if (group == null) return NotFound();

            _context.AccountGroups.Remove(group);
            await _context.SaveChangesAsync();
            return Ok(true);
        }
    }
}
