using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateProjectDto
    {
        [Required]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("image")]
        public CreateImageDto? Image { get; set; }

        [Required]
        [JsonPropertyName("creator_id")]
        public string? CreatorId { get; set; }
    }
}
