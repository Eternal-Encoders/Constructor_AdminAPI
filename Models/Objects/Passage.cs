using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Objects
{
    public class Passage
    {
        [BsonId]
        [Required]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        //Такой же, как у точки
        public string Id { get; set; }

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

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime? UpdatedAt { get; set; }
    }
}
