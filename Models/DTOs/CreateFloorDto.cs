using Constructor_API.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs
{
    public class CreateFloorDto
    {
        [JsonPropertyName("floor_number")]
        [JsonRequired]
        [Required]
        public int FloorNumber { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("building_id")]
        [JsonRequired]
        [Required]
        [StringLength(24)]
        public string BuildingId { get; set; }

        [JsonPropertyName("width")]
        [JsonRequired]
        [Required]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        [JsonRequired]
        [Required]
        public int Height { get; set; }

        [JsonPropertyName("service")]
        [JsonRequired]
        [Required]
        public Service[] Service { get; set; }

        [JsonPropertyName("graph")]
        public Dictionary<string, GraphPointFromFloorDto>? GraphPoints { get; set; }

        [JsonPropertyName("forces")]
        public Forces[]? Forces { get; set; }
    }
}
