using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.DTOs.Create;
using Constructor_API.Models.Objects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Update
{
    public class UpdateFloorDto
    {
        [JsonPropertyName("floor_number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? FloorNumber { get; set; }

        [JsonPropertyName("floor_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FloorName { get; set; }

        [JsonPropertyName("building_id")]
        [ObjectId]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BuildingId { get; set; }

        [JsonPropertyName("image_id")]
        [ObjectId]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ImageId { get; set; }

        [JsonPropertyName("width")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Height { get; set; }

        [JsonPropertyName("services")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Service[]? Services { get; set; }

        [JsonPropertyName("graph_points")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CreateGraphPointFromFloorDto[]? GraphPoints { get; set; }

        [JsonPropertyName("rooms")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Room[]? Rooms { get; set; }

        [JsonPropertyName("forces")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Forces[]? Forces { get; set; }
    }
}
