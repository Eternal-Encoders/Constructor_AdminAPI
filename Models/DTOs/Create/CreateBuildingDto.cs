using System.Text.Json.Serialization;
using Constructor_API.Models.Entities;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using Constructor_API.Models.Objects;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateBuildingDto
    {
        [JsonPropertyName("project_id")]
        [ObjectId]
        [Required]
        public string? ProjectId { get; set; }

        [JsonPropertyName("name")]
        [Required]
        [MinLength(1)]
        public string? Name { get; set; }

        [JsonPropertyName("displayable_name")]
        [Required]
        [MinLength(1)]
        public string? DisplayableName { get; set; }

        [JsonPropertyName("url")]
        [Required]
        [MinLength(1)]
        public string? Url { get; set; }

        [JsonPropertyName("latitude")]
        [Required]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [Required]
        public double? Longitude { get; set; }

        //[JsonPropertyName("image")]
        //public CreateImageDto? Image { get; set; }

        [JsonPropertyName("gps")]
        public GPS? GPS { get; set; }
    }
}
