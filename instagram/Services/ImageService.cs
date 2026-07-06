using Microsoft.AspNetCore.Connections;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace instagram.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ImageService(HttpClient httpClient,
                            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<UploadResult> UploadImageAsync(IFormFile file)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://upload.imagekit.io/api/v1/files/upload");

            var privateKey = _configuration["ImageKit:PrivateKey"];

            var auth = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{privateKey}:")
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", auth);

            var content = new MultipartFormDataContent();

            using var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", file.FileName);
            content.Add(new StringContent(file.FileName), "fileName");

            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Image upload failed: {json}");

            var result = JsonSerializer.Deserialize<UploadResult>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null || string.IsNullOrEmpty(result.Url))
                throw new Exception("Upload succeeded but URL is null");

            return result;
        }

        public async Task<bool> DeleteImageAsync(string fileId)
        {
            var privateKey = _configuration["ImageKit:PrivateKey"];

            var auth = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{privateKey}:"));

            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"https://api.imagekit.io/v1/files/{fileId}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", auth);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}