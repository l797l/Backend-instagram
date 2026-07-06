using Microsoft.AspNetCore.Http;

namespace instagram.Services
{
    public interface IImageService
    {
        Task<UploadResult> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string fileId);

    }
}