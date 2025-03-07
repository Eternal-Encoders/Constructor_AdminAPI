using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs
{
    public class CreateIconDto
    {
        [JsonPropertyName("url")]
        [JsonRequired]
        public string Url { get; set; }

        [JsonPropertyName("alt")]
        public string? Alt { get; set; }
    }
}
