using Constructor_API.Models.InnerObjects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Constructor_API.Helpers.Attributes;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateRoomFromFloorDto
    {
        [JsonPropertyName("id")]
        [Required]
        [ObjectId]
        public string? Id { get; set; }

        [JsonPropertyName("x")]
        [Required]
        public double? X { get; set; }

        [JsonPropertyName("y")]
        [Required]
        public double? Y { get; set; }

        [JsonPropertyName("width")]
        [Required]
        public double? Width { get; set; }

        [JsonPropertyName("height")]
        [Required]
        public double? Height { get; set; }

        [JsonPropertyName("fill")]
        [Required]
        public string? Fill { get; set; }

        [JsonPropertyName("stroke")]
        [Required]
        public string? Stroke { get; set; }

        [JsonPropertyName("children")]
        public RoomChild[]? Children { get; set; }

        [JsonPropertyName("doors")]
        [Required]
        public Door[]? Doors { get; set; }
    }
}
