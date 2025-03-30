using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class PaidFeature
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        [BsonRequired]
        public string Description { get; set; }

        [BsonElement("short_description")]
        [JsonPropertyName("short_description")]
        public string? ShortDescription { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }
    }
}
