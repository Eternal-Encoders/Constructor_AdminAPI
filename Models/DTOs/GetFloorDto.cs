using Constructor_API.Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs
{
    public class GetFloorDto
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("floor_number")]
        [JsonPropertyName("floor_number")]
        public int? FloorNumber { get; set; }

        [BsonElement("building_id")]
        [JsonPropertyName("building_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? BuildingId { get; set; }

        [BsonElement("building")]
        [JsonPropertyName("building")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Building { get; set; }

        [BsonElement("width")]
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [BsonElement("height")]
        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [BsonElement("service")]
        [JsonPropertyName("service")]
        public Service[]? Service { get; set; }

        [BsonElement("rooms")]
        [JsonPropertyName("rooms")]
        public Room[]? Rooms { get; set; }

        [BsonElement("graph")]
        [JsonPropertyName("graph")]
        public GraphPoint[]? GraphPoints { get; set; }

        [BsonElement("forces")]
        [JsonPropertyName("forces")]
        [BsonIgnoreIfNull]
        public Forces[]? Forces { get; set; }
    }
}
