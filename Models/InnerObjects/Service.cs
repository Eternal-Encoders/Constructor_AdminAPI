using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.InnerObjects
{
    public class Service
    {
        [BsonElement("x")]
        [JsonPropertyName("x")]
        [Required]
        [BsonRequired]
        public double? X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [Required]
        [BsonRequired]
        public double? Y { get; set; }

        [BsonElement("data")]
        [JsonPropertyName("data")]
        public string? Data { get; set; }

        [BsonElement("stroke")]
        [JsonPropertyName("stroke")]
        [Required]
        [BsonRequired]
        public string? Stroke { get; set; }

        [BsonElement("fill")]
        [JsonPropertyName("fill")]
        [BsonIgnoreIfNull]
        public string? Fill { get; set; }
    }
}
