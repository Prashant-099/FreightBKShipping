using Microsoft.AspNetCore.Mvc;

namespace FreightBKShipping.Controllers
{
    using FreightBKShipping.Data;
    using FreightBKShipping.DTOs;
    using FreightBKShipping.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

        [Authorize]
        [ApiController]
        [Route("api/[controller]")]
        public class SendWpMailController : BaseController
        {
            private readonly AppDbContext _context;
            private readonly ISieveProcessor _sieveProcessor;
            private readonly SentWpEmailService _SentWpEmailService;


        public SendWpMailController(AppDbContext context, ISieveProcessor sieveProcessor, SentWpEmailService sentWpEmailService)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
            _SentWpEmailService = sentWpEmailService; // correct
        }

        // GET all (with pagination & filtering)
        [HttpGet]
            public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
            {
                var currentPage = sieveModel.Page ?? 1;
                var pageSize = sieveModel.PageSize ?? 10;

                var query = _context.SendWpMails.AsNoTracking(); // no FilterByCompany needed

                var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

                var totalRecords = await filteredQuery.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var paged = await filteredQuery
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new SendWpMailDto
                    {
                        SendWpMailId = x.SendWpMailId,
                        SendWpMailUserName = x.SendWpMailUserName,
                        SendWpMailWpToken = x.SendWpMailWpToken,
                        SendWpMailBalanceToken = x.SendWpMailBalanceToken,
                        SendWpMailCompanyid = x.SendWpMailCompanyid,
                     SendWpMailWpBalanceToken = x.SendWpMailWpBalanceToken

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
                    data = paged
                });
            }

            // GET by id
            [HttpGet("{id}")]
            public async Task<ActionResult<SendWpMailDto>> Get(int id)
            {
                var record = await _context.SendWpMails.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.SendWpMailId == id);

                if (record == null) return NotFound();

                return Ok(new SendWpMailDto
                {
                    SendWpMailId = record.SendWpMailId,
                    SendWpMailUserName = record.SendWpMailUserName,
                    SendWpMailWpToken = record.SendWpMailWpToken,
                    SendWpMailBalanceToken = record.SendWpMailBalanceToken,
                    SendWpMailCompanyid = record.SendWpMailCompanyid,
                    SendWpMailWpBalanceToken = record.SendWpMailWpBalanceToken

                }); 
            }

        // GET all by logged-in company only
        [HttpGet("company")]    
        public async Task<ActionResult<List<SendWpMailDto>>> GetByCompany()
        {
            var companyId = GetCompanyId(); // logged-in company

            var records = await _context.SendWpMails
                .AsNoTracking()
                .Where(x => x.SendWpMailCompanyid == companyId)
                .Select(x => new SendWpMailDto
                {
                    SendWpMailId = x.SendWpMailId,
                    SendWpMailUserName = x.SendWpMailUserName,
                    SendWpMailWpToken = x.SendWpMailWpToken,
                    SendWpMailBalanceToken = x.SendWpMailBalanceToken,
                    SendWpMailCompanyid = x.SendWpMailCompanyid,
                    SendWpMailWpBalanceToken = x.SendWpMailWpBalanceToken

                })
                .ToListAsync();

            return Ok(records);
        }


            // POST (create new)
            [HttpPost]
            public async Task<IActionResult> Create(SendWpMailAddDto dto)
            {
            int companyId = GetCompanyId();

            // Check if company has HasWp = true
            var company = await _context.companies
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);
            if (company == null)
            {
                return BadRequest(new { message = "Company not found." });
            }

            if (company.HasWhatsapp != true)
            {
                return BadRequest(new { message = "WhatsApp feature not enabled for this company." });
            }

            bool exists = await _context.SendWpMails
                    .AnyAsync(x => x.SendWpMailUserName == dto.SendWpMailUserName);

                if (exists)
                {
                    return Conflict(new { message = "A record with the same username already exists." });
                }

                var entity = new SendWpMail
                {
                    SendWpMailUserName = dto.SendWpMailUserName,
                    SendWpMailWpToken = dto.SendWpMailWpToken,
                    SendWpMailBalanceToken = dto.SendWpMailBalanceToken,
                    SendWpMailWpBalanceToken = dto.SendWpMailWpBalanceToken,
                    SendWpMailCompanyid = GetCompanyId()

                };

                _context.SendWpMails.Add(entity);
                await _context.SaveChangesAsync();

                return Ok(new { entity.SendWpMailId });
            }

            // PUT (update)
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, SendWpMailDto dto)
            {
                var entity = await _context.SendWpMails.FindAsync(id);
                if (entity == null) return NotFound();

                entity.SendWpMailUserName = dto.SendWpMailUserName;
                entity.SendWpMailWpToken = dto.SendWpMailWpToken;
                entity.SendWpMailBalanceToken = dto.SendWpMailBalanceToken;
                entity.SendWpMailWpBalanceToken = dto.SendWpMailWpBalanceToken;
            await _context.SaveChangesAsync();
                return NoContent();
            }

            // DELETE
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var entity = await _context.SendWpMails.FirstOrDefaultAsync(x => x.SendWpMailId== id);
                if (entity == null) return NotFound();

                _context.SendWpMails.Remove(entity);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        //============================================================================
        [HttpGet("config")]
        public async Task<IActionResult> GetConfig()
        {
            var companyId = GetCompanyId();
            var config = await _context.SendWpMails
                .Where(x => x.SendWpMailCompanyid == companyId)
                .FirstOrDefaultAsync();

            if (config == null) return NotFound();
            return Ok(config);
        }
        [HttpPost("config")]
        public async Task<IActionResult> SaveConfig([FromBody] SendWpMailConfigDto dto)
        {
            var companyId = GetCompanyId();

            // Check agar record exist hai
            var config = await _context.SendWpMails
                .FirstOrDefaultAsync(x => x.SendWpMailCompanyid == companyId);

            if (config == null)
            {
                // --- Naya record banega ---
                config = new SendWpMail
                {
                    SendWpMailCompanyid = companyId,
                    SendWpMailEmailFrom = dto.EmailFrom,
                    SendWpMailSmtpServer = dto.SmtpServer,
                    SendWpMailSmtpPort = dto.SmtpPort,
                    SendWpMailEmailUser = dto.EmailUser,
                    SendWpMailEmailPass = dto.EmailPass,
                    SendWpMailEnableSsl = dto.EnableSsl,
                    SendWpMailCreated = DateTime.UtcNow
                };
                _context.SendWpMails.Add(config);
            }
            else
            {
                // --- Purana record update hoga ---
                config.SendWpMailEmailFrom = dto.EmailFrom;
                config.SendWpMailSmtpServer = dto.SmtpServer;
                config.SendWpMailSmtpPort = dto.SmtpPort;
                config.SendWpMailEmailUser = dto.EmailUser;
                config.SendWpMailEmailPass = dto.EmailPass;
                config.SendWpMailEnableSsl = dto.EnableSsl;
                config.SendWpMailUpdated = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok(config);
        }

        // POST send email
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDto dto)
        {
            var companyId = GetCompanyId();
            var mailConfig = await _context.SendWpMails
                .FirstOrDefaultAsync(x => x.SendWpMailCompanyid == companyId);

            if (mailConfig == null)
                return BadRequest("Email configuration not found for your company.");

            try
            {
                await _SentWpEmailService.SendEmailAsync(mailConfig, dto);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Email sending failed: {ex.Message}" });
            }
        }

    }




}
