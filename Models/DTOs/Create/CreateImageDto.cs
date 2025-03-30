using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateImageDto
    {
        [JsonPropertyName("url")]
        [JsonRequired]
        [Required]
        public string Url { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("filesize")]
        public string? Filesize { get; set; }
    }
}
