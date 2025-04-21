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
        //private static readonly Dictionary<string, GraphPointType> pointTypes = new Dictionary<string, GraphPointType>()
        //{
        //    ["corridor"] = new GraphPointType { "corridor" },
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

        [JsonPropertyName("is_pass_free")]
        [Required]
        public bool? IsPassFree { get; set; }

        [ObjectId]
        [JsonPropertyName("transition_id")]
        public string? TransitionId { get; set; }
    }
}
