//using Azure.Storage.Blobs;
//using FreightBKShipping.Controllers;

//using Microsoft.AspNetCore.Mvc;

//[Route("api/[controller]")]
//[ApiController]
//public class BillUploadController : BaseController
//{
//    private readonly BlobContainerClient _containerClient;

//    public BillUploadController(IConfiguration configuration)
//    {
//        string connectionString = configuration["AzureStorage:ConnectionString"];
//        string containerName = configuration["AzureStorage:ContainerName"];

//        _containerClient = new BlobContainerClient(connectionString, containerName);
//            _containerClient.CreateIfNotExists();
//    }

//    [HttpPost("upload-pdf-file")]
//    public async Task<IActionResult> UploadPdfFile(IFormFile file, string billid, string billtype, int companyId)
//    {
//        if (file == null || file.Length == 0)
//            return BadRequest("File is missing");

//        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
//            return BadRequest("Only PDF supported.");

//        // 👇 Always override from BaseController (claims/user context)
//        companyId = GetCompanyId();

//        string blobUrl;
//        try
//        {
//            blobUrl = await UploadCertificateAsync(file, billid, billtype, companyId);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, "Upload error: " + ex.Message);
//        }

//        return Ok(new { url = blobUrl });
//    }

//    [NonAction]
//    public async Task<string> UploadCertificateAsync(IFormFile file, string billId, string billType, int companyId)
//    {
//        try
//        {


//            var extension = Path.GetExtension(file.FileName).ToLower();
//            if (extension != ".pdf")
//                throw new InvalidOperationException("Only PDF files are allowed.");

//            var safeCertType = string.Concat(billType.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");

//            // ✅ int ko string me convert karke safe bana do
//            var safeCompanyId = companyId.ToString();

//            // Original filename without extension
//            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
//            var safeFileName = string.Concat(originalFileName.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");


//            var blobName = $"billpdf/{safeCompanyId}/{safeCertType}/{safeFileName}_{billId}{extension}";

//            var blobClient = _containerClient.GetBlobClient(blobName);
//            using var stream = file.OpenReadStream();
//            await blobClient.UploadAsync(stream, overwrite: true);

//            return blobClient.Uri.ToString();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception($"Azure Blob upload failed: {ex.Message}", ex);
//        }
//    }
//}

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FreightBKShipping.Controllers;
using FreightBKShipping.Data;
using FreightBKShipping.Models;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FreightBKShipping.Interfaces;
[Route("api/[controller]")]
[ApiController]
public class FileUploadController : BaseController
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;
    private readonly ISasUrlService _sasUrlService;

    //private static readonly HashSet<string> AllowedExtensions =
    //    new(StringComparer.OrdinalIgnoreCase)
    //    {
    //        ".pdf", ".jpg", ".jpeg", ".png", ".gif",
    //        ".doc", ".docx", ".xls", ".xlsx",
    //        ".txt", ".csv", ".zip", ".rar"
    //    };
    // Replace old AllowedExtensions + MaxFileSize block with this

    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
        ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".webp",
        ".mp4", ".webm", ".mov",
        ".doc", ".docx", ".xls", ".xlsx",
        ".txt", ".csv", ".zip", ".rar"
        };

    private static readonly HashSet<string> ImageExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
        };

    private static readonly HashSet<string> VideoExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
        ".mp4", ".webm", ".mov"
        };

    private const long MaxDocumentFileSize = 10 * 1024 * 1024; // 10 MB
    private const long MaxImageFileSize = 5 * 1024 * 1024;     // 5 MB
    private const long MaxVideoFileSize = 25 * 1024 * 1024;    // 25 MB

    //private const long MaxFileSize = 10 * 1024 * 1024;

    public FileUploadController(
      IConfiguration configuration,
      AppDbContext dbContext,
      ISasUrlService sasUrlService)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _sasUrlService = sasUrlService;

        string connectionString =
            configuration["AzureStorage:ConnectionString"]
            ?? throw new ArgumentNullException("AzureStorage:ConnectionString is missing");

        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    // ===================== UPLOAD =====================
    // ===================== ADMIN UPLOAD (companyId ticket se) =====================
    // Replace AdminUploadFile action with this
    [HttpPost("admin-upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AdminUploadFile(
        [FromForm] FileUploadRequest request,
        [FromForm] int ticketCompanyId)
    {
        if (!TryValidateFile(request.File, out _, out var validationError))
            return BadRequest(validationError);

        var result = await UploadFileAsync(
            request.File,
            request.Category,
            request.SubCategory,
            request.ReferenceId,
            ticketCompanyId);

        return Ok(result);
    }

    // Replace UploadFile action with this
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
    {
        if (!TryValidateFile(request.File, out _, out var validationError))
            return BadRequest(validationError);

        int companyId = GetCompanyId();

        var result = await UploadFileAsync(
            request.File,
            request.Category,
            request.SubCategory,
            request.ReferenceId,
            companyId);

        return Ok(result);
    }
    // Add these helper methods in controller
    private bool TryValidateFile(IFormFile? file, out string extension, out string error)
    {
        extension = string.Empty;
        error = string.Empty;

        if (file == null || file.Length == 0)
        {
            error = "File is missing";
            return false;
        }

        extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
        {
            error = $"File type not allowed. Allowed: {string.Join(", ", AllowedExtensions.OrderBy(x => x))}";
            return false;
        }

        var maxSize = GetMaxFileSizeByExtension(extension);
        if (file.Length > maxSize)
        {
            var maxMb = maxSize / (1024 * 1024);
            error = $"File too large. Max allowed for {extension} is {maxMb} MB.";
            return false;
        }

        return true;
    }

    private static long GetMaxFileSizeByExtension(string extension)
    {
        if (VideoExtensions.Contains(extension))
            return MaxVideoFileSize;

        if (ImageExtensions.Contains(extension))
            return MaxImageFileSize;

        return MaxDocumentFileSize;
    }


    [NonAction]
    private async Task<FileUploadResult> UploadFileAsync(
      IFormFile file,
      string category,
      string? subCategory,
      string? referenceId,
      int companyId)
    {
        // 🔹 get company name (container name)
        var companyName = await _dbContext.companies
            .Where(c => c.CompanyId == companyId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(companyName))
            throw new Exception("Company not found");

        var containerName = SanitizeContainerName(companyName);

        var containerClient =
            _blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        var extension = Path.GetExtension(file.FileName).ToLower();

        var safeSubCategory = string.IsNullOrWhiteSpace(subCategory)
            ? "general"
            : SanitizeFileName(subCategory);

        var safeFileName =
            SanitizeFileName(Path.GetFileNameWithoutExtension(file.FileName));

        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

        var uniqueId = string.IsNullOrWhiteSpace(referenceId)
            ? Guid.NewGuid().ToString("N")[..8]
            : SanitizeFileName(referenceId);

        var blobName =
            $"{safeSubCategory}/{safeFileName}_{timestamp}_RefId-{uniqueId}_Company-{companyId}{extension}";

        var blobClient = containerClient.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = GetContentType(extension)
            }
        });

        // 🔐 safe UploadedBy
        var uploadedBy =
            User?.Identity?.IsAuthenticated == true
                ? User.Identity!.Name!
                : "system";

        var document = new DocumentsSaved
        {
            CompanyId = companyId,

            Category = category,
            SubCategory = subCategory,

            ReferenceType = category,
            ReferenceId = referenceId,

            ContainerName = containerName,
            BlobName = blobName,
            BlobUrl = blobClient.Uri.ToString(),

            OriginalFileName = file.FileName,
            StoredFileName = Path.GetFileName(blobName),
            FileExtension = extension,
            ContentType = GetContentType(extension),
            FileSizeBytes = file.Length,

            UploadedBy = uploadedBy,   // 🔥 FIXED
            UploadedAt = DateTime.UtcNow
        };

        _dbContext.DocumentsSaved.Add(document);
        await _dbContext.SaveChangesAsync();

        return new FileUploadResult
        {
            Success = true,
            DocumentId = document.DocumentId,
            Url = document.BlobUrl,
            BlobName = blobName,
            ContainerName = containerName,
            FileName = file.FileName,
            FileSize = file.Length,
            ContentType = document.ContentType,
            Category = category,
            SubCategory = subCategory,
            ReferenceId = referenceId
        };
    }


    // ===================== LIST FROM DB =====================

  [HttpGet("documents")]
public async Task<IActionResult> GetDocuments(
    string category,
    string? referenceId = null,
    bool adminOverride = false)
{
    int companyId = GetCompanyId();

    // ✅ adminOverride=true hone pe companyId filter mat lagao
    IQueryable<DocumentsSaved> query;

    if (adminOverride)
    {
        query = _dbContext.DocumentsSaved
            .Where(d => d.Category == category && !d.IsDeleted);
    }
    else
    {
        query = _dbContext.DocumentsSaved
            .Where(d => d.CompanyId == companyId
                     && d.Category == category
                     && !d.IsDeleted);
    }

    if (!string.IsNullOrWhiteSpace(referenceId))
        query = query.Where(d => d.ReferenceId == referenceId);

    var docs = await query
        .OrderByDescending(d => d.UploadedAt)
        .ToListAsync();

    return Ok(docs);
}

    // ===================== DELETE =====================

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFile(long documentId)
    {
        int companyId = GetCompanyId();

        var document = await _dbContext.DocumentsSaved
            .FirstOrDefaultAsync(d =>
                d.DocumentId == documentId &&
                d.CompanyId == companyId &&
                !d.IsDeleted);

        if (document == null)
            return NotFound();

        var containerClient =
            _blobServiceClient.GetBlobContainerClient(document.ContainerName);

        var blobClient =
            containerClient.GetBlobClient(document.BlobName);

        await blobClient.DeleteIfExistsAsync();

        document.IsDeleted = true;
        document.DeletedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return Ok(new { success = true });
    }



    // ===================== Preview! =====================
    // Preview endpoint: force inline + resolved content type
    [HttpGet("preview/{documentId}")]
    public async Task<IActionResult> Preview(long documentId)
    {
        int companyId = GetCompanyId();

        var doc = await _dbContext.DocumentsSaved.FirstOrDefaultAsync(d =>
            d.DocumentId == documentId &&
            d.CompanyId == companyId &&
            !d.IsDeleted);

        if (doc == null) return NotFound();

        var containerClient = _blobServiceClient.GetBlobContainerClient(doc.ContainerName);
        var blobClient = containerClient.GetBlobClient(doc.BlobName);
        if (!await blobClient.ExistsAsync()) return NotFound();

        var response = await blobClient.DownloadStreamingAsync();
        var contentType = ResolveDocumentContentType(doc);

        Response.Headers["Accept-Ranges"] = "bytes";
        Response.Headers["Content-Disposition"] = "inline";

        return File(response.Value.Content, contentType, enableRangeProcessing: true);
    }


    // SAS endpoint: send inline + content type overrides
    [HttpGet("sas/{documentId}")]
    public async Task<IActionResult> GetSasUrl(long documentId, int expiresInMinutes = 10)
    {
        int companyId = GetCompanyId();

        var doc = await _dbContext.DocumentsSaved.FirstOrDefaultAsync(d =>
            d.DocumentId == documentId &&
            d.CompanyId == companyId &&
            !d.IsDeleted);

        if (doc == null) return NotFound();

        var contentType = ResolveDocumentContentType(doc);

        var sasUrl = _sasUrlService.GenerateReadSasUrl(
            doc.ContainerName,
            doc.BlobName,
            expiresInMinutes,
            responseContentType: contentType,
            responseContentDisposition: "inline");

        return Ok(new
        {
            documentId,
            sasUrl,
            expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes)
        });
    }


    // GET api/superadmin/admin-documents
    [HttpGet("admin-documents")]
    public async Task<IActionResult> GetAdminDocuments(
        string category,
        string? referenceId = null)
    {
        var docs = await _dbContext.DocumentsSaved
            .Where(d => d.Category == category && !d.IsDeleted)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(referenceId))
            docs = docs.Where(d => d.ReferenceId == referenceId).ToList();

        return Ok(docs);
    }

    // GET api/superadmin/admin-sas/{documentId}
    // Admin SAS endpoint bhi same override ke saath
    [HttpGet("admin-sas/{documentId}")]
    public async Task<IActionResult> GetAdminSasUrl(long documentId, int expiresInMinutes = 60)
    {
        var doc = await _dbContext.DocumentsSaved
            .FirstOrDefaultAsync(d => d.DocumentId == documentId && !d.IsDeleted);

        if (doc == null) return NotFound();

        var contentType = ResolveDocumentContentType(doc);

        var sasUrl = _sasUrlService.GenerateReadSasUrl(
            doc.ContainerName,
            doc.BlobName,
            expiresInMinutes,
            responseContentType: contentType,
            responseContentDisposition: "inline");

        return Ok(new { documentId, sasUrl, expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes) });
    }


    // ===================== HELPERS =====================
    // FileUploadController helper
    private string ResolveDocumentContentType(DocumentsSaved doc)
    {
        var current = doc.ContentType?.Trim();
        if (!string.IsNullOrWhiteSpace(current) &&
            !current.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
        {
            return current;
        }

        var ext = (doc.FileExtension ?? Path.GetExtension(doc.OriginalFileName) ?? "").ToLowerInvariant();
        return GetContentType(ext);
    }

    private string SanitizeContainerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "general";

        return new string(name
            .ToLower()
            .Replace(" ", "-")
            .Replace("_", "-")
            .Where(c => char.IsLetterOrDigit(c) || c == '-')
            .ToArray());
    }

    private string SanitizeFileName(string name)
    {
        return string.Concat(
            name.Split(Path.GetInvalidFileNameChars()))
            .Replace(" ", "_");
    }

    // Replace GetContentType with this
    private string GetContentType(string ext) => ext switch
    {
        ".pdf" => "application/pdf",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".gif" => "image/gif",
        ".webp" => "image/webp",
        ".mp4" => "video/mp4",
        ".webm" => "video/webm",
        ".mov" => "video/quicktime",
        ".doc" => "application/msword",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        ".xls" => "application/vnd.ms-excel",
        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ".txt" => "text/plain",
        ".csv" => "text/csv",
        ".zip" => "application/zip",
        ".rar" => "application/x-rar-compressed",
        _ => "application/octet-stream"
    };

}



public class FileUploadResult
{
    public bool Success { get; set; }
    public long DocumentId { get; set; }
    public string Url { get; set; }
    public string BlobName { get; set; }
    public string ContainerName { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; }
    public string Category { get; set; }
    public string? SubCategory { get; set; }
    public string? ReferenceId { get; set; }
}
public class FileUploadRequest
{
    public IFormFile File { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string? SubCategory { get; set; }
    public string? ReferenceId { get; set; }
}

//-----------------------------------------------------------------------------------

