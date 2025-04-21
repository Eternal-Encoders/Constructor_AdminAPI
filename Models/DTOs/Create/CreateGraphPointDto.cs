using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Entities;
using Constructor_API.Models.Objects;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Create
{
    [PointType]
    public class CreateGraphPointDto
    {
        //private static readonly Dictionary<string, string> pointTypes = new Dictionary<string, string>()
        //{
        //    ["corridor"] = "corridor",
        //    ["exit"] = "exit",
        //    ["fire-exit"] = "fire-exit",
        //    ["stair"] = "stair",
        //    ["elevator"] = "elevator",
        //    ["escalator"] = "escalator",
        //    ["toilet-m"] = "toilet-m",
        //    ["toilet-w"] = "toilet-w",
        //    ["cafe"] = "cafe",
        //    ["dinning"] = "dinning",
        //    ["restaurant"] = "restaurant",
        //    ["wardrobe"] = "wardrobe",
        //    ["other"] = "other"
        //};

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
        public string[]? Links { get; set; }

        [JsonPropertyName("types")]
        [Required]
        public string[]? Types { get; set; }

        [JsonPropertyName("names")]
        [Required]
        public string? Name { get; set; }

        [JsonPropertyName("synonyms")]
        [Required]
        public string[]? Synonyms { get; set; }

        [JsonPropertyName("floor_id")]
        [ObjectId]
        [Required]
        public string? FloorId { get; set; }

        [JsonPropertyName("time")]
        //public Schedule[]? Time { get; set; }
        public Day[]? Time { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("info")]
        public string? Info { get; set; }

        [JsonPropertyName("is_pass_free")]
        [Required]
        public bool? IsPassFree { get; set; }

        [JsonPropertyName("transition_id")]
        [ObjectId]
        public string? TransitionId { get; set; }
    }
}
