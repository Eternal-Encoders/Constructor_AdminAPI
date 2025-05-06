using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Objects;
using Constructor_API.Models.Entities;

namespace Constructor_API.Models.DTOs.Create
{
    //[PointType]
    public class CreateGraphPointFromFloorDto
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

        [JsonPropertyName("links")]
        [Required]
        [ObjectId]
        public string[]? Links { get; set; }

        [JsonPropertyName("types")]
        [Required]
        public string[]? Types { get; set; }

        [JsonPropertyName("name")]
        [Required]
        public string? Name { get; set; }

        [JsonPropertyName("synonyms")]
        [Required]
        public string[]? Synonyms { get; set; }

        [JsonPropertyName("time")]
        //[BsonIgnoreIfNull]
        //public Schedule[]? Time { get; set; }
        public Day[]? Time { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("info")]
        public string? Info { get; set; }

        [JsonPropertyName("route_active")]
        [Required]
        public bool? RouteActive { get; set; }

        [JsonPropertyName("search_active")]
        [Required]
        public bool? SearchActive { get; set; }

        [ObjectId]
        [JsonPropertyName("transition_id")]
        public string? TransitionId { get; set; }
    }
}
