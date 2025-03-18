using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Constructor_API.Core.Shared;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Models.Entities
{
    public class Project: IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("buildings")]
        [JsonPropertyName("buildings")]
        public string[] BuildingIds { get; set; }

        [JsonPropertyName("description")]
        [BsonElement("description")]
        public string? Description { get; set; }

        [StringLength(24)]
        [JsonPropertyName("image_id")]
        [BsonElement("image_id")]
        public string? ImageId { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
