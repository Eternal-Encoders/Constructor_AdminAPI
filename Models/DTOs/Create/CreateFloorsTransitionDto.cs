using Constructor_API.Helpers.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateFloorsTransitionDto
    {
        [JsonPropertyName("name")]
        [Required]
        [MinLength(1)]
        public string? Name { get; set; }

        [JsonPropertyName("building_id")]
        [Required]
        [ObjectId]
        public string? BuildingId { get; set; }

        [JsonPropertyName("direction")]
        public string? Direction { get; set; }
    }
}
