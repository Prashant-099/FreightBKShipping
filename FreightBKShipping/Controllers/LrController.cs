using FreightBKShipping.Controllers;
using FreightBKShipping.Interfaces;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LrController : BaseController
{
    private readonly ILrService _service;

    public LrController(ILrService service)
    {
        _service = service;
    }

    // 🔎 SEARCH
    [HttpGet("search")]
    public async Task<IActionResult> SearchByPartyAndDate(
        int partyId,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var result = await _service.SearchByPartyAndDate(partyId, fromDate, toDate);
        return Ok(result);
    }

    // 📋 GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lrs = await _service.GetAll();
        return Ok(lrs);
    }

    // 📄 GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lr = await _service.GetById(id);

        if (lr == null)
            return NotFound();

        return Ok(lr);
    }

    // 🔥 UPSERT (Insert / Update)
    //[HttpPost("save")]
    //public async Task<IActionResult> Save([FromBody] Lr model)
    //{
    //    if (model == null)
    //        return BadRequest();

    //    model.LrCompanyId = GetCompanyId();
    //    model.LrUpdatedByUserId = GetUserId();
    //    model.LrUpdated = DateTime.UtcNow;

    //    if (model.LrId == 0)
    //    {
    //        model.LrAddedByUserId = GetUserId();
    //        model.LrCreated = DateTime.UtcNow;
    //        model.LrStatus = 1;

    //        var created = await _service.Create(model);
    //        return Ok(created);
    //    }
    //    else
    //    {
    //        var updated = await _service.Update(model);
    //        if (!updated)
    //            return NotFound();

    //        return Ok(model);
    //    }
    //}
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] LrEntryDto dto)
    {
        if (dto == null)
            return BadRequest();

        dto.Main.LrCompanyId = GetCompanyId();
        dto.Main.LrUpdatedByUserId = GetUserId();
        dto.Main.LrUpdated = DateTime.UtcNow;

        if (dto.Main.LrId == 0)
        {
            dto.Main.LrAddedByUserId = GetUserId();
            dto.Main.LrCreated = DateTime.UtcNow;
            dto.Main.LrStatus = 1;

            var created = await _service.Create(dto.Main, dto.Details, dto.Journals);
            return Ok(created);
        }
        else
        {
            var updated = await _service.Update(dto.Main, dto.Details, dto.Journals);
            if (!updated)
                return NotFound();

            return Ok(dto.Main);
        }
    }


    // ❌ DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.Delete(id);

        if (!deleted)
            return NotFound();

        return Ok();
    }

    public class LrEntryDto
    {
        public Lr Main { get; set; } = new();
        public List<LRDetail> Details { get; set; } = new();
        public List<LRJournal> Journals { get; set; } = new();
    }
}