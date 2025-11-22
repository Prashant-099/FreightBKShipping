

using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace FreightBKShipping.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WpMailConfigController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public WpMailConfigController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany(_context.wpMailConfigs.AsNoTracking(), "CompanyId");

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedData = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new WpMailConfigDto
                {
                    Id = w.Id,
                    MsgType = w.MsgType,
                    TemplateText = w.TemplateText,
                    MailSub = w.MailSub,
                    CompanyId = w.CompanyId,
                    IsDefault=w.IsDefault
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
                data = pagedData
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WpMailConfigDto>> Get(int id)
        {
            var config = await FilterByCompany(_context.wpMailConfigs.AsNoTracking(), "CompanyId")
                .FirstOrDefaultAsync(w => w.Id == id);

            if (config == null) return NotFound();

            return Ok(new WpMailConfigDto
            {
                Id = config.Id,
                MsgType = config.MsgType,
                TemplateText = config.TemplateText,
                MailSub = config.MailSub,
                CompanyId = config.CompanyId,
                IsDefault=config.IsDefault
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(WpMailConfigAddDto dto)
        {
            var config = new WpMailConfig
            {
                MsgType = dto.MsgType,
                TemplateText = dto.TemplateText,
                MailSub = dto.MailSub,
                CompanyId = GetCompanyId(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                CreateUid = GetUserId().ToString(),
                IsDefault = dto.IsDefault
            };

            if (dto.IsDefault)
            {
                // unset other defaults for this company AND same MsgType
                var existingDefaults = await _context.wpMailConfigs
                    .Where(w => w.CompanyId == config.CompanyId
                                && w.MsgType == config.MsgType
                                && w.IsDefault == true)
                    .ToListAsync();

                foreach (var w in existingDefaults)
                    w.IsDefault = false;
            }

            _context.wpMailConfigs.Add(config);
            await _context.SaveChangesAsync();

            return Ok(new { config.Id });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WpMailConfigDto dto)
        {
            var config = await _context.wpMailConfigs.FindAsync(id);
            if (config == null) return NotFound();

            config.MsgType = dto.MsgType;
            config.TemplateText = dto.TemplateText;
            config.MailSub = dto.MailSub;
            config.Updated = DateTime.UtcNow;
            config.EditedUid = GetUserId().ToString();
            config.CompanyId = GetCompanyId();
            config.IsDefault = dto.IsDefault;

            if (dto.IsDefault)
            {
                var existingDefaults = await _context.wpMailConfigs
                    .Where(w => w.CompanyId == config.CompanyId
                                && w.MsgType == config.MsgType
                                && w.Id != config.Id
                                && w.IsDefault == true)
                    .ToListAsync();

                foreach (var w in existingDefaults)
                    w.IsDefault = false;
            }

            await _context.SaveChangesAsync();
            return Ok(true);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var config = await FilterByCompany(_context.wpMailConfigs.AsNoTracking(), "CompanyId")
                .FirstOrDefaultAsync(w => w.Id == id);

            if (config == null) return NotFound();

            _context.wpMailConfigs.Remove(config);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        // GET api/wpMailConfig/default?msgType=CERTI&companyId=1
        [HttpGet("default")]
        public async Task<ActionResult<WpMailConfigDto>> GetDefaultConfig([FromQuery] string msgType, [FromQuery] int? companyId = null)
        {
            if (string.IsNullOrEmpty(msgType))
                return BadRequest("msgType is required.");

            // Filter by company if provided, else current user's company
            var query = _context.wpMailConfigs.AsNoTracking().AsQueryable();

            if (companyId.HasValue)
                query = query.Where(w => w.CompanyId == companyId.Value);
            else
                query = query.Where(w => w.CompanyId == GetCompanyId());

            // Filter by msgType
            //var config = await query
            //    .Where(w => w.MsgType == msgType)
            //    .OrderBy(w => w.Id) // first one as default
            //    .FirstOrDefaultAsync();

            // Filter by msgType + only default
            var config = await query
                .Where(w => w.MsgType == msgType && w.IsDefault == true)
                .FirstOrDefaultAsync();

            if (config == null) return NotFound();

            return Ok(new WpMailConfigDto
            {
                Id = config.Id,
                MsgType = config.MsgType,
                TemplateText = config.TemplateText,
                MailSub = config.MailSub,
                CompanyId = config.CompanyId,
                IsDefault=config.IsDefault
            });
        }


        [HttpPost("render")]
        public async Task<IActionResult> RenderTemplate([FromBody] RenderTemplateRequest dto)
        {
            var config = await _context.wpMailConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == dto.ConfigId);

            if (config == null) return NotFound("Config not found");

            var template = config.TemplateText ?? "";
            foreach (var kv in dto.Parameters)
                template = template.Replace("{" + kv.Key + "}", kv.Value);

            return Ok(new { Message = template });
        }

        public class RenderTemplateRequest
        {
            public int ConfigId { get; set; }
            public Dictionary<string, string> Parameters { get; set; }
        }

    }
}
