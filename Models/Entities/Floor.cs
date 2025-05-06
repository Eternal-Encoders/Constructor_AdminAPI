using Constructor_API.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Driver.GeoJsonObjectModel;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Models.Objects;
using Constructor_API.Helpers.Attributes;

namespace Constructor_API.Models.Entities
{
    public class Floor : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("number")]
        [JsonPropertyName("number")]
        [BsonRequired]
        public int FloorNumber { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string FloorName { get; set; }

        [BsonElement("building_id")]
        [JsonPropertyName("building_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        [ObjectId]
        public string BuildingId { get; set; }

        [BsonIgnore]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Building? Building { get; set; }

        [BsonElement("image_ids")]
        [JsonPropertyName("image_ids")]
        [ObjectId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string[] ImageIds { get; set; }

        //[BsonElement("images")]
        //[JsonPropertyName("images")]
        //[BsonIgnoreIfNull]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string[]? Images { get; set; }

        [BsonElement("width")]
        [JsonPropertyName("width")]
        [BsonRequired]
        public int Width { get; set; }

        [BsonElement("height")]
        [JsonPropertyName("height")]
        [BsonRequired]
        public int Height { get; set; }

        [BsonElement("decorations")]
        [JsonPropertyName("decorations")]
        //[BsonRequired]
        public Decoration[] Decorations { get; set; }

        [BsonElement("rooms")]
        [JsonPropertyName("rooms")]
        //[BsonRequired]
        public Room[] Rooms { get; set; }

        [BsonElement("graph")]
        [JsonPropertyName("graph")]
        [BsonRequired]
        [ObjectId]
        public string[] GraphPoints { get; set; }

        [BsonElement("forces")]
        [JsonPropertyName("forces")]
        [BsonIgnoreIfNull]
        public Forces[]? Forces { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        [JsonPropertyName("updated_by")]
        [ObjectId]
        public string? UpdatedBy { get; set; }
    }
}
