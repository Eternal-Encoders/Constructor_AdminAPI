using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.InnerObjects
{
    public class Coordinates
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
    }
}
