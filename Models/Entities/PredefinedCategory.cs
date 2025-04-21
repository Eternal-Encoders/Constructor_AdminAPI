using Constructor_API.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class PredefinedCategory : IAggregateRoot
    {
        [BsonId]
        [JsonPropertyName("id")]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
