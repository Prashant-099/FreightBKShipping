using Azure.Storage.Blobs;
using FreightBKShipping.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BillUploadController : BaseController
{
    private readonly BlobContainerClient _containerClient;

    public BillUploadController(IConfiguration configuration)
    {
        string connectionString = configuration["AzureStorage:ConnectionString"];
        string containerName = configuration["AzureStorage:ContainerName"];

        _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
    }

    [HttpPost("upload-pdf-file")]
    public async Task<IActionResult> UploadPdfFile(IFormFile file, string billid, string billtype, int companyId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is missing");

        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            return BadRequest("Only PDF supported.");

        // 👇 Always override from BaseController (claims/user context)
        companyId = GetCompanyId();

        string blobUrl;
        try
        {
            blobUrl = await UploadCertificateAsync(file, billid, billtype, companyId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Upload error: " + ex.Message);
        }

        return Ok(new { url = blobUrl });
    }

    [NonAction]
    public async Task<string> UploadCertificateAsync(IFormFile file, string billId, string billType, int companyId)
    {
        try
        {
           

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");

            var safeCertType = string.Concat(billType.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");

            // ✅ int ko string me convert karke safe bana do
            var safeCompanyId = companyId.ToString();

            // Original filename without extension
            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var safeFileName = string.Concat(originalFileName.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");
        

            var blobName = $"billpdf/{safeCompanyId}/{safeCertType}/{safeFileName}_{billId}{extension}";

            var blobClient = _containerClient.GetBlobClient(blobName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Azure Blob upload failed: {ex.Message}", ex);
        }
    }
}
