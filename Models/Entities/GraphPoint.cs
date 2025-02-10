using ConstructorAdminAPI.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.Entities
{
    public class GraphPoint : MongoRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [BsonElement("x")]
        [JsonPropertyName("x")]
        public double X { get; set; }
        [BsonElement("y")]
        [JsonPropertyName("y")]
        public double Y { get; set; }
        [BsonElement("links")]
        [JsonPropertyName("links")]
        public string[] Links { get; set; }
        [BsonElement("types")]
        [JsonPropertyName("types")]
        public string[] Types { get; set; } //PointTypes
        [BsonElement("names")]
        [JsonPropertyName("names")]
        public string[] Names { get; set; }
        [BsonElement("floor")]
        [JsonPropertyName("floor")]
        public int Floor { get; set; }
        [BsonElement("building")]
        [JsonPropertyName("building")]
        public string Building { get; set; }
        [BsonElement("time")]
        [JsonPropertyName("time")]
        //[BsonIgnoreIfNull]
        //public Schedule[]? Time { get; set; }
        public Day[]? Time { get; set; }
        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [BsonElement("info")]
        [JsonPropertyName("info")]
        public string Info { get; set; }
        [BsonElement("menuId")]
        [JsonPropertyName("menuId")]
        [BsonIgnoreIfNull]
        public string? MenuId { get; set; }
        [BsonElement("isPassFree")]
        [JsonPropertyName("isPassFree")]
        [BsonIgnoreIfNull]
        public string? IsPassFree { get; set; }
        [BsonElement("stairId")]
        [JsonPropertyName("stairId")]
        [BsonIgnoreIfNull]
        public string? StairId { get; set; }
    }
}
