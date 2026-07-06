using System.Text.Json.Serialization;

namespace instagram.Services
{
    public class UploadResult
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("fileId")]
        public string FileId { get; set; }
    }
}