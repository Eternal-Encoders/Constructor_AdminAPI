using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.InnerObjects
{
    public class Door
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

        [BsonElement("width")]
        [JsonPropertyName("width")]
        [Required]
        [BsonRequired]
        public double? Width { get; set; }

        [BsonElement("height")]
        [JsonPropertyName("height")]
        [Required]
        [BsonRequired]
        public double? Height { get; set; }

        [BsonElement("fill")]
        [JsonPropertyName("fill")]
        [Required]
        [BsonRequired]
        public string? Fill { get; set; }
    }
}
