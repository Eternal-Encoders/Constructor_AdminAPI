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
        [JsonPropertyName("name")]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        [BsonRequired]
        public string? Name { get; set; }

        [JsonPropertyName("displayable_name")]
        [BsonElement("displayable_name")]
        [BsonRequired]
        public string? DisplayableName { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        [BsonIgnoreIfNull]
        public string? Description { get; set; }
    }
}
