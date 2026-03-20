namespace FreightBKShipping.Interfaces
{
    public interface ISasUrlService
    {
        string GenerateReadSasUrl(
      string containerName,
      string blobName,
      int expiresInMinutes = 10,
      string? responseContentType = null,
      string? responseContentDisposition = null);

    }

}