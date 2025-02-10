using ConstructorAdminAPI.Core.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.Entities
{
    public class Building : MongoRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [BsonElement("displayableName")]
        [JsonPropertyName("displayableName")]
        public string DisplayableName { get; set; }
        [BsonElement("minFloor")]
        [JsonPropertyName("minFloor")]
        public int MinFloor { get; set; }
        [BsonElement("maxFloor")]
        [JsonPropertyName("maxFloor")]
        public int MaxFloor { get; set; }
        [BsonElement("url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [BsonElement("latitude")]
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [BsonElement("longitude")]
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        [BsonElement("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}
