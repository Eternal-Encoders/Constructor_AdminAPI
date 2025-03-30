using Constructor_API.Core.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class PredefinedGraphPointType : IAggregateRoot
    {
        [BsonId]
        [JsonPropertyName("id")]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [BsonElement("category")]
        [JsonPropertyName("category")]
        public string? Category { get; set; }
    }
}
