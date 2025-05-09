using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Constructor_API.Models.Objects;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateFloorDto
    {
        [JsonPropertyName("index")]
        [Required]
        public int? Index { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("building_id")]
        [Required]
        [ObjectId]
        public string? BuildingId { get; set; }

        //[JsonPropertyName("image_ids")]
        //[ObjectId]
        //[Required]
        //public string[]? ImageIds { get; set; }

        //[JsonPropertyName("width")]
        //[Required]
        //public int? Width { get; set; }

        //[JsonPropertyName("height")]
        //[Required]
        //public int? Height { get; set; }

        ////Пройдет ли пустой массив?
        //[JsonPropertyName("decorations")]
        //[Required]
        //public Decoration[]? Decorations { get; set; }

        //[JsonPropertyName("graph_points")]
        //[Required]
        //public CreateGraphPointFromFloorDto[]? GraphPoints { get; set; }

        //[JsonPropertyName("rooms")]
        //[Required]
        //public Room[]? Rooms { get; set; }

        ////[JsonPropertyName("graph")]
        ////public GraphPointFromFloorDto[]? GraphPoints { get; set; }

        //[JsonPropertyName("forces")]
        ////[Required]
        //public Forces[]? Forces { get; set; }
    }
}
