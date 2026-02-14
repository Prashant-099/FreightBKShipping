namespace FreightBKShipping.Interfaces
{
    public interface ISasUrlService
    {
        string GenerateReadSasUrl(
            string containerName,
            string blobName,
            int minutesValid = 10);
    }

}