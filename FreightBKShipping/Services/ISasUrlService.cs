namespace FreightBKShipping.Services
{
    public interface ISasUrlService
    {
        string GenerateReadSasUrl(
            string containerName,
            string blobName,
            int minutesValid = 10);
    }

}