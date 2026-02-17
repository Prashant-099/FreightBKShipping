
using FreightBKShipping.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreightBKShipping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LrController : ControllerBase
    {
        private readonly ILrService _service;

        public LrController(ILrService service)
        {
            _service = service;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByPartyAndDate(
            int partyId,
            DateTime? fromDate,
            DateTime? toDate)
        {
            var result = await _service.SearchByPartyAndDate(partyId, fromDate, toDate);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lrs = await _service.GetAll();
            return Ok(lrs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lr = await _service.GetById(id);

            if (lr == null)
                return NotFound();

            return Ok(lr);
        }
    }
}
