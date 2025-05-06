using Constructor_API.Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Objects;

namespace Constructor_API.Models.DTOs.Read
{
    public class GetFloorDto
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [ObjectId]
        public string Id { get; set; }

        [BsonElement("number")]
        [JsonPropertyName("number")]
        public int FloorNumber { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string? FloorName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("building_id")]
        [JsonPropertyName("building_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [ObjectId]
        public string BuildingId { get; set; }

        //[BsonElement("building")]
        //[JsonPropertyName("building")]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Building { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("width")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("height")]
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("decorations")]
        [JsonPropertyName("decorations")]
        public Decoration[]? Decorations { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("rooms")]
        [JsonPropertyName("rooms")]
        public Room[]? Rooms { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("graph")]
        [JsonPropertyName("graph")]
        public GraphPoint[]? GraphPoints { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("forces")]
        [JsonPropertyName("forces")]
        [BsonIgnoreIfNull]
        public Forces[]? Forces { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [BsonElement("updated_by")]
        [JsonPropertyName("updated_by")]
        [ObjectId]
        public string? UpdatedBy { get; set; }
    }
}
