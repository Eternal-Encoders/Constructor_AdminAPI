using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class FloorConnection : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [ObjectId]
        public string? Id { get; set; }

        [BsonElement("building_id")]
        [JsonPropertyName("building_id")]
        [Required]
        [ObjectId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? BuildingId { get; set; }

        [BsonElement("links")]
        [JsonPropertyName("links")]
        [Required]
        public string[]? Links { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [Required]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [Required]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        [JsonPropertyName("updated_by")]
        [ObjectId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UpdatedBy { get; set; }
    }
}
