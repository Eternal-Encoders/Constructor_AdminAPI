using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Objects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Update
{
    public class UpdateFloorDto
    {
        [JsonPropertyName("index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Index { get; set; }

        [JsonPropertyName("name")]
        [MinLength(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("building_id")]
        [ObjectId]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BuildingId { get; set; }

        //[JsonPropertyName("image_ids")]
        //[ObjectId]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public string[]? ImageIds { get; set; }

        [JsonPropertyName("width")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Height { get; set; }

        [JsonPropertyName("decorations")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Decoration[]? Decorations { get; set; }

        [JsonPropertyName("graph_points")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CreateGraphPointFromFloorDto[]? GraphPoints { get; set; }

        [JsonPropertyName("rooms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Room[]? Rooms { get; set; }
    }
}
