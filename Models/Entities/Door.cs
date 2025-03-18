using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class Door
    {
        [BsonElement("x")]
        [JsonPropertyName("x")]
        [Required]
        public double X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [Required]
        public double Y { get; set; }

        [BsonElement("width")]
        [JsonPropertyName("width")]
        [Required]
        public double Width { get; set; }

        [BsonElement("height")]
        [JsonPropertyName("height")]
        [Required]
        public double Height { get; set; }

        [BsonElement("fill")]
        [JsonPropertyName("fill")]
        [Required]
        public string Fill { get; set; }
    }
}
