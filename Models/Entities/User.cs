using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        [JsonPropertyName("email")]
        [BsonRequired]
        public string Email { get; set; }

        [BsonElement("hash")]
        [JsonPropertyName("hash")]
        [BsonRequired]
        public string Hash { get; set; }

        [BsonElement("projects")]
        [JsonPropertyName("projects")]
        public string[] ProjectIds { get; set; }
    }
}
