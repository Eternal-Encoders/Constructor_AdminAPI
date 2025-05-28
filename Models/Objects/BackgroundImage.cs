using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Objects
{
    public class BackgroundImage
    {
        [Required]
        [BsonRequired]
        [BsonElement("image_id")]
        [JsonPropertyName("image_id")]
        public string? ImageId { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("x")]
        [JsonPropertyName("x")]
        public double? X { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("y")]
        [JsonPropertyName("y")]
        public double? Y { get; set; }

        //[Required]
        //[BsonRequired]
        //[BsonElement("width")]
        //[JsonPropertyName("width")]
        //public double? Width { get; set; }

        //[Required]
        //[BsonRequired]
        //[BsonElement("height")]
        //[JsonPropertyName("height")]
        //public double? Height { get; set; }

        [Required]
        [BsonRequired]
        [BsonElement("multiplier")]
        [JsonPropertyName("multiplier")]
        public double? Multiplier { get; set; }
    }
}
