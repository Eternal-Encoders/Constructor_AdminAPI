using Constructor_API.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class Stair : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [BsonElement("building_id")]
        [JsonPropertyName("building_id")]
        [Required]
        [StringLength(24)]
        public string BuildingId { get; set; }

        [BsonElement("links")]
        [JsonPropertyName("links")]
        public string[]? Links { get; set; }
    }
}
