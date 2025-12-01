using Microsoft.AspNetCore.Mvc;
using FreightBKShipping.Data;
using FreightBKShipping.DTOs;
using FreightBKShipping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System.Text;
namespace FreightBKShipping.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportDataController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public ReportDataController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            if (_context == null)
            {
                throw new Exception("_context is NULL — DbContext is not injected properly.");
            }

            if (_context.Reportdata == null)
            {
                throw new Exception("_context.Reportdata is NULL — DbSet is missing in AppDbContext.");
            }

            var query = _context.Reportdata.AsNoTracking();
            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);
            var totalRecords = await filteredQuery.CountAsync();

            var page = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var data = await filteredQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReportDataDto
                {
                    ReportDataId = r.ReportDataId,
                    DocType = r.DocType,
                    LayoutData = r.LayoutData,
                    FormatName = r.FormatName,
                    VchId = r.VchId,
                    CopyFormat = r.CopyFormat,
                    NextFormat = r.NextFormat,
                    CompanyId = r.CompanyId,
                    Status = r.Status
                })
                .ToListAsync();

            return Ok(new
            {
                pagination = new
                {
                    page,
                    pageSize,
                    totalRecords,
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                },
                data
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _context.Reportdata.FindAsync(id);
            if (entity == null) return NotFound();

            return Ok(new ReportDataDto
            {
                ReportDataId = entity.ReportDataId,
                DocType = entity.DocType,
                LayoutData = entity.LayoutData,
                FormatName = entity.FormatName,
                VchId = entity.VchId,
                CopyFormat = entity.CopyFormat,
                NextFormat = entity.NextFormat,
                CompanyId = entity.CompanyId,
                Status = entity.Status
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReportDataAddDto dto)
        {
            var userId = GetUserId()?.ToString() ?? "system";

            var entity = new Reportdata
            {
                DocType = dto.DocType,
                LayoutData = dto.LayoutData,
                FormatName = dto.FormatName,
                VchId = dto.VchId,
                CopyFormat = dto.CopyFormat,
                NextFormat = dto.NextFormat,
                CompanyId = GetCompanyId(),
                Status = dto.Status,
                CreateUid = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            _context.Reportdata.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(new { entity.ReportDataId });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ReportDataDto dto)
        {
            var entity = await _context.Reportdata.FindAsync(id);
            if (entity == null) return NotFound();

            var userId = GetUserId()?.ToString() ?? "system";

            entity.DocType = dto.DocType;
            entity.LayoutData = dto.LayoutData;
            entity.FormatName = dto.FormatName;
            entity.VchId = dto.VchId;
            entity.CopyFormat = dto.CopyFormat;
            entity.NextFormat = dto.NextFormat;
            entity.CompanyId = GetCompanyId();
            entity.Status = dto.Status;
            entity.EditedUid = userId;
            entity.Updated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Reportdata
                .FirstOrDefaultAsync(r => r.ReportDataId == id && r.CompanyId == GetCompanyId());

            if (entity == null) return NotFound();
            _context.Reportdata.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpGet("layout/{id:int}")]
        public async Task<IActionResult> GetReportLayout(int id)
        {
            try
            {
                var layout = await _context.Reportdata
                    .Where(r => r.ReportDataId == id)
                    .Select(r => r.LayoutData)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(layout))
                {
                    return NotFound(new { error = "Report layout not found." });
                }

                var bytes = Encoding.UTF8.GetBytes(layout);
                return File(bytes, "application/xml");
            }
            catch (Exception ex)
            {
                // Maybe return a generic error message to the client
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "An error occurred while retrieving the report layout. Please try again later." });
            }
        }


        // Designer endpoints remain mostly unchanged with minor fixes:
        [HttpGet("designer/reports")]
        public async Task<IActionResult> GetDesignerReportsList()
        {
            var companyId = GetCompanyId();
            var reports = await _context.Reportdata 
                .Where(r => r.CompanyId == companyId && !string.IsNullOrEmpty(r.FormatName))
                .Select(r => r.FormatName)
                .Distinct()
                .ToListAsync();

            return Ok(reports.ToDictionary(x => x, x => x));
        }

        [HttpGet("designer/reports/{reportName}")]
        public async Task<IActionResult> GetDesignerReportData(string reportName)
        {
            var companyId = GetCompanyId();
            var report = await _context.Reportdata
                .FirstOrDefaultAsync(r => r.FormatName == reportName.Trim() && r.CompanyId == companyId);

            if (report?.LayoutData == null)
            {
                var emptyTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
<XtraReportsLayoutSerializer SerializerVersion=""24.1.3.0"" Ref=""1"" ControlType=""DevExpress.XtraReports.UI.XtraReport, DevExpress.XtraReports.v24.1"" Name=""XtraReport"" PageWidth=""850"" PageHeight=""1100"" Version=""24.1"">
  <Bands>
    <Item1 Ref=""2"" ControlType=""DevExpress.XtraReports.UI.TopMarginBand, DevExpress.XtraReports.v24.1"" Name=""TopMargin"" />
    <Item2 Ref=""3"" ControlType=""DevExpress.XtraReports.UI.BottomMarginBand, DevExpress.XtraReports.v24.1"" Name=""BottomMargin"" />
    <Item3 Ref=""4"" ControlType=""DevExpress.XtraReports.UI.DetailBand, DevExpress.XtraReports.v24.1"" Name=""Detail"" />
  </Bands>
  <Version>24.1</Version>
  <DataMember>Query</DataMember>
</XtraReportsLayoutSerializer>";
                return Content(emptyTemplate, "application/xml", Encoding.UTF8);
            }

            return Content(report.LayoutData, "application/xml", Encoding.UTF8);
        }

        [HttpPost("designer/reports/{reportName}")]
        public async Task<IActionResult> SaveDesignerReportData(string reportName)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var layoutData = await reader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(layoutData))
                return BadRequest("Layout data is empty");

            var companyId = GetCompanyId();
            var userId = GetUserId()?.ToString() ?? "system";

            var existing = await _context.Reportdata
                .FirstOrDefaultAsync(r => r.FormatName == reportName.Trim() && r.CompanyId == companyId);

            if (existing != null)
            {
                existing.LayoutData = layoutData;
                existing.EditedUid = userId;
                existing.Updated = DateTime.UtcNow;
            }
            else
            {
                _context.Reportdata.Add(new Reportdata
                {
                    FormatName = reportName.Trim(),
                    LayoutData = layoutData,
                    DocType = "Report",
                    CompanyId = companyId,
                    CreateUid = userId,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Status = true
                });
            }
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Report saved successfully" });
        }

        [HttpDelete("designer/reports/{reportName}")]
        public async Task<IActionResult> DeleteDesignerReport(string reportName)
        {
            var companyId = GetCompanyId();
            var report = await _context.Reportdata
                .FirstOrDefaultAsync(r => r.FormatName == reportName.Trim() && r.CompanyId == companyId);

            if (report == null) return NotFound("Report not found");

            _context.Reportdata.Remove(report);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }


    }
}
