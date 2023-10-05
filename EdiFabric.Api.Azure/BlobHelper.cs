using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EdiFabric.Api.Azure
{
    public class BlobHelper
    {        
        public static async Task<List<string>> ListFromCache(string containerName)
        {
            var result = new List<string>();

            var cloudBlobContainer = GetBlobServiceClient().GetBlobContainerClient(containerName);

            if (cloudBlobContainer.Exists())
            {
                var blobs = cloudBlobContainer.GetBlobsAsync(BlobTraits.None, BlobStates.None, string.Empty);
                await foreach (var blob in blobs)
                {
                    result.Add(blob.Name);
                }
            }

            return result;
        }

        public static async Task<Stream> ReadFromCache(string containerName, string blobName)
        {
            var cloudBlobContainer = GetBlobServiceClient().GetBlobContainerClient(containerName);

            if (!cloudBlobContainer.Exists())
                throw new InvalidDataException($"Can't find container {containerName}.");

            var cloudBlockBlob = cloudBlobContainer.GetBlobClient(blobName);
            if (!cloudBlockBlob.Exists())
                throw new InvalidDataException($"Can't find blob {blobName}.");

            var modelStream = new MemoryStream();
            await cloudBlockBlob.DownloadToAsync(modelStream);
            modelStream.Position = 0;

            return modelStream;
        }

        public static async Task WriteToCache(string containerName, string blobName, Stream data)
        {
            var cloudBlobContainer = GetBlobServiceClient().GetBlobContainerClient(containerName);
            cloudBlobContainer.CreateIfNotExists();

            var cloudBlockBlob = cloudBlobContainer.GetBlobClient(blobName);
            await cloudBlockBlob.UploadAsync(data, true);
        }

        private static BlobServiceClient GetBlobServiceClient()
        {
            var options = new BlobClientOptions();
            //options.Diagnostics.IsLoggingEnabled = false;
            //options.Diagnostics.IsTelemetryEnabled = false;
            //options.Diagnostics.IsDistributedTracingEnabled = false;
            //options.Retry.MaxRetries = 0;

            return new BlobServiceClient(Configuration.AzureStorageConnectionString, options);
        }
    }
}
