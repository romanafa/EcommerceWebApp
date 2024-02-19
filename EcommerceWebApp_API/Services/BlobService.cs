
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EcommerceWebApp_API.Services
{
    public class BlobService : IBlobService
    {

        // Inject Blob client
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> GetBlob(string blobName, string container)
        {
            // Find container by container name
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);

            // Look for the blob inside container by blob name
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            // return URL that we save in database
            return blobClient.Uri.AbsoluteUri;
        }
        public async Task<string> CreateBlob(string blobName, string container, IFormFile image)
        {
            // Find container by container name
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);

            // Look for the blob inside container by blob name
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            // Set content type
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = image.ContentType
            };

            // Upload image to blob storage
            var response = await blobClient.UploadAsync(image.OpenReadStream(), httpHeaders);

            // Return URL that we save in database
            if (response != null)
            {
                return await GetBlob(blobName, container);
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> DeleteBlob(string blobName, string container)
        {
            // Find container by container name
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(container);

            // Look for the blob inside container by blob name
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            return await blobClient.DeleteIfExistsAsync();
        }

        
    }
}
