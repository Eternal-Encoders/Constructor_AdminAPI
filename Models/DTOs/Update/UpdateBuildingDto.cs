using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Objects;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Update
{
    public class UpdateBuildingDto
    {
        [ObjectId]
        [JsonPropertyName("project_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProjectId { get; set; }

        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        [JsonPropertyName("displayable_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DisplayableName { get; set; }

        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; }

        [JsonPropertyName("latitude")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Longitude { get; set; }

        [ObjectId]
        [JsonPropertyName("image_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ImageId { get; set; }

        [JsonPropertyName("gps")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public GPS? GPS { get; set; }
    }
}
