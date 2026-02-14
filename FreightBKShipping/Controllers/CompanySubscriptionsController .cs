using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreightBKShipping.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Add roles/policies if needed
    public class CompanySubscriptionsController : ControllerBase
    {
        private readonly ICompanySubscriptionService _service;

        public CompanySubscriptionsController(ICompanySubscriptionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var data = await _service.GetByCompanyIdAsync(companyId);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanySubscription model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanySubscription model)
        {
            var updated = await _service.UpdateAsync(id, model);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
