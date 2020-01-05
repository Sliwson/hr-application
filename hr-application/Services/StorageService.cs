using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace hr_application.Services
{
    public class StorageService
    {
        private readonly string connectionString;

        public StorageService(IConfiguration config)
        {
            connectionString = config.GetConnectionString("StorageBlob");   
        }

        public async Task<string> StoreFile(IFormFile file)
        {
            var containerClient = await GetContainerClient();
            var guid = Guid.NewGuid().ToString();
            var fileName = guid + ".pdf";
            var blobClient = containerClient.GetBlobClient(fileName);

            var uploadFileStream = file.OpenReadStream();
            await blobClient.UploadAsync(uploadFileStream);
            uploadFileStream.Close();

            return guid;
        }
        
        public async Task<string> GetDownloadUrl(string guid)
        {
            var containerClient = await GetContainerClient();
            var filename = guid + ".pdf";
            return containerClient.Uri.AbsoluteUri + "/" + filename;
        }

        private async Task<BlobContainerClient> GetContainerClient()
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient;

            try
            {
                containerClient = blobServiceClient.GetBlobContainerClient("pdf");
            }
            catch
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync("pdf");
            }

            return containerClient;
        }
    }
}
