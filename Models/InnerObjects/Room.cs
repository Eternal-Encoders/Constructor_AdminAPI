using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.InnerObjects
{
    public class Room
    {
        [BsonId]
        [Required]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        //Такой же, как у точки
        public string? Id { get; set; }

        [JsonPropertyName("x")]
        [BsonElement("x")]
        [BsonRequired]
        [Required]
        public double? X { get; set; }

        [JsonPropertyName("y")]
        [BsonElement("y")]
        [BsonRequired]
        [Required]
        public double? Y { get; set; }

        [JsonPropertyName("width")]
        [BsonElement("width")]
        [BsonRequired]
        [Required]
        public double? Width { get; set; }

        [JsonPropertyName("height")]
        [BsonElement("height")]
        [BsonRequired]
        [Required]
        public double? Height { get; set; }

        //[JsonPropertyName("points")]
        //[BsonElement("points")]
        //[BsonIgnoreIfNull]
        //public Coordinates[]? Points { get; set; }

        [JsonPropertyName("fill")]
        [BsonElement("fill")]
        [BsonRequired]
        [Required]
        public string? Fill { get; set; }

        [JsonPropertyName("stroke")]
        [BsonElement("stroke")]
        [BsonRequired]
        [Required]
        public string? Stroke { get; set; }

        [JsonPropertyName("floor_id")]
        [BsonElement("floor_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [ObjectId]
        [BsonRequired]
        public string? FloorId { get; set; }

        [JsonPropertyName("children")]
        [BsonElement("children")]
        [BsonIgnoreIfNull]
        public RoomChild[]? Children { get; set; }

        [JsonPropertyName("doors")]
        [BsonElement("doors")]
        [BsonRequired]
        [Required]
        public Door[]? Doors { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull]
        [ObjectId]
        public string? UpdatedBy { get; set; }
    }
}
