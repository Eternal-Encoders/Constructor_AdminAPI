using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Objects
{
    public class Room
    {
        [Required]
        [BsonRequired]
        [ObjectId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        //Такой же, как у точки
        public string? Id { get; set; }

        //[JsonPropertyName("x")]
        //[BsonElement("x")]
        //[BsonRequired]
        //[Required]
        //public double? X { get; set; }

        //[JsonPropertyName("y")]
        //[BsonElement("y")]
        //[BsonRequired]
        //[Required]
        //public double? Y { get; set; }

        //[JsonPropertyName("width")]
        //[BsonElement("width")]
        //[BsonRequired]
        //[Required]
        //public double? Width { get; set; }

        //[JsonPropertyName("height")]
        //[BsonElement("height")]
        //[BsonRequired]
        //[Required]
        //public double? Height { get; set; }

        [JsonPropertyName("points")]
        [BsonElement("points")]
        [BsonRequired]
        [Required]
        public Coordinates[]? Points { get; set; }

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
        [ObjectId]
        [BsonRequired]
        [Required]
        public string? FloorId { get; set; }

        [JsonPropertyName("children")]
        [BsonElement("children")]
        [BsonIgnoreIfNull]
        public RoomMarker[]? Children { get; set; }

        [JsonPropertyName("passages")]
        [BsonElement("passages")]
        [BsonRequired]
        [Required]
        public Passage[]? Passages { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        [Required]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        [Required]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        [JsonPropertyName("updated_by")]
        [ObjectId]
        public string? UpdatedBy { get; set; }
    }
}
