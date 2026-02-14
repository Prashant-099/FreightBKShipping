using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FreightBKShipping.Interfaces;


namespace FreightBKShipping.Services
{
 
    public class SasUrlService : ISasUrlService
    {
        private readonly string _connectionString;
        private readonly BlobServiceClient _blobServiceClient;

        public SasUrlService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureStorage:ConnectionString"]
                ?? throw new ArgumentNullException("AzureStorage:ConnectionString missing");

            _blobServiceClient = new BlobServiceClient(_connectionString);
        }

        public string GenerateReadSasUrl(
            string containerName,
            string blobName,
            int minutesValid = 10)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(minutesValid)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var credential = new StorageSharedKeyCredential(
                _blobServiceClient.AccountName,
                GetAccountKey(_connectionString));

            var sasToken = sasBuilder
                .ToSasQueryParameters(credential)
                .ToString();

            return $"{blobClient.Uri}?{sasToken}";
        }

        private string GetAccountKey(string connectionString)
        {
            return connectionString
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .First(p => p.StartsWith("AccountKey=", StringComparison.OrdinalIgnoreCase))
                .Split('=', 2)[1];
        }
    }

}
