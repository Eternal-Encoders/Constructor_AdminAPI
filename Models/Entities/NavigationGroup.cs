using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Constructor_API.Core.Shared;

namespace Constructor_API.Models.Entities
{
    public class NavigationGroup : IAggregateRoot
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
    }
}
