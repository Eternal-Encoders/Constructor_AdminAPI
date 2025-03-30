using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Constructor_API.Models.InnerObjects;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateFloorDto
    {
        [JsonPropertyName("floor_number")]
        [Required]
        public int? FloorNumber { get; set; }

        [JsonPropertyName("floor_name")]
        public string? FloorName { get; set; }

        [JsonPropertyName("building_id")]
        [Required]
        [ObjectId]
        public string? BuildingId { get; set; }

        [JsonPropertyName("image_id")]
        [ObjectId]
        public string? ImageId { get; set; }

        [JsonPropertyName("width")]
        [Required]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        [Required]
        public int? Height { get; set; }

        //Пройдет ли пустой массив?
        [JsonPropertyName("services")]
        [Required]
        public Service[]? Services { get; set; }

        [JsonPropertyName("graph_points")]
        [Required]
        public CreateGraphPointFromFloorDto[]? GraphPoints { get; set; }

        [JsonPropertyName("rooms")]
        [Required]
        public CreateRoomFromFloorDto[]? Rooms { get; set; }

        //[JsonPropertyName("graph")]
        //public GraphPointFromFloorDto[]? GraphPoints { get; set; }

        [JsonPropertyName("forces")]
        public Forces[]? Forces { get; set; }
    }
}
