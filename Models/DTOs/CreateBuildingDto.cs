using System.Text.Json.Serialization;
using Constructor_API.Models.Entities;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Constructor_API.Models.DTOs
{
    [MinMaxFloorValidation]
    public class CreateBuildingDto
    {
        [JsonPropertyName("project_id")]
        [ObjectId]
        [Required]
        public string? ProjectId { get; set; }

        [JsonPropertyName("name")]
        [JsonRequired]
        [Required]
        public string? Name { get; set; }

        [JsonPropertyName("displayable_name")]
        [JsonRequired]
        [Required]
        public string? DisplayableName { get; set; }

        [JsonPropertyName("min_floor")]
        [JsonRequired]
        [Required]
        public int? MinFloor { get; set; }

        [JsonPropertyName("max_floor")]
        [JsonRequired]
        [Required]
        public int? MaxFloor { get; set; }

        //[JsonPropertyName("floors")]
        //public string[]? FloorIds { get; set; }

        [JsonPropertyName("url")]
        [JsonRequired]
        [Required]
        public string? Url { get; set; }

        [JsonPropertyName("latitude")]
        [JsonRequired]
        [Required]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [JsonRequired]
        [Required]
        public double? Longitude { get; set; }

        [JsonPropertyName("icon")]
        public CreateImageDto? Icon { get; set; }

        [JsonPropertyName("gps")]
        public GPS? GPS { get; set; }
    }
}
