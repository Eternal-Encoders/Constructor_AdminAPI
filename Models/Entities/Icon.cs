using ConstructorAdminAPI.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.Entities
{
    public class Icon : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [BsonElement("alt")]
        [JsonPropertyName("alt")]
        public string Alt { get; set; }
    }
}
