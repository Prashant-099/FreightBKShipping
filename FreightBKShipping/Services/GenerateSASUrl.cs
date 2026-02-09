using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
public class SasUrlGenerator
{
    private string GenerateReadSasUrl(
       string connectionString,
       string containerName,
       string blobName,
       int minutesValid = 10)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b", // blob
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(minutesValid)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var credential = new StorageSharedKeyCredential(
            blobServiceClient.AccountName,
            GetAccountKeyFromConnectionString(connectionString)
        );

        var sasToken = sasBuilder.ToSasQueryParameters(credential).ToString();

        return $"{blobClient.Uri}?{sasToken}";
    }


    private string GetAccountKeyFromConnectionString(string connectionString)
    {
        var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
        return parts.First(p => p.StartsWith("AccountKey=", StringComparison.OrdinalIgnoreCase))
                    .Split('=', 2)[1];
    }
}