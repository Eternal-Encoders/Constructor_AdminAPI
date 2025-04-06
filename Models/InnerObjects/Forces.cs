using Constructor_API.Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.InnerObjects
{
    public class Forces
    {
        [BsonElement("point")]
        [JsonPropertyName("point")]
        [Required]
        [BsonRequired]
        public Coordinates? Point { get; set; }

        [BsonElement("force")]
        [JsonPropertyName("force")]
        [Required]
        [BsonRequired]
        public Coordinates? Force { get; set; }
    }
}
