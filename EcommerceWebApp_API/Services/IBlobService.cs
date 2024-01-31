namespace EcommerceWebApp_API.Services
{
    public interface IBlobService
    {
        // actions to get/delete/upload the image file
        Task<string> GetBlob(string blobName, string container);
        Task<bool> DeleteBlob(string blobName, string container);
        Task<string> CreateBlob(string blobName, string container, IFormFile image);

    }
}
