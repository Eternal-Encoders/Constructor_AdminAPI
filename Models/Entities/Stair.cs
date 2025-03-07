using Constructor_API.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class Stair : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [BsonElement("stairPoint")]
        [JsonPropertyName("stairPoint")]
        public string StairPoint { get; set; }

        [BsonElement("institute")]
        [JsonPropertyName("institute")]
        public string Building { get; set; }

        [BsonElement("links")]
        [JsonPropertyName("links")]
        [BsonIgnoreIfNull]
        public string[]? Links { get; set; }
    }
}
