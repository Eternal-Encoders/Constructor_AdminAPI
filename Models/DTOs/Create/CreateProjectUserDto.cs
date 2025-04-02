using Constructor_API.Helpers.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateProjectUserDto
    {
        [Required]
        [JsonPropertyName("user_id")]
        [ObjectId]
        public string? UserId { get; set; }

        [Required]
        [JsonPropertyName("project_id")]
        [ObjectId]
        public string? ProjectId { get; set; }

        [Required]
        [JsonPropertyName("project_role")]
        public string? ProjectRole { get; set; }
    }
}
