using Constructor_API.Helpers.Attributes;
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
        [JsonPropertyName("description")]
        public string? ShortDescription { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("description")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("description")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }
    }
}
