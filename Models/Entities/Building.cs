using Constructor_API.Core.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class Building : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("displayableName")]
        [JsonPropertyName("displayableName")]
        //[BsonElement("userId")]
        //[JsonPropertyName("userId")]
        //public string UserId { get; set; }
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

        [BsonElement("gps")]
        [JsonPropertyName("gps")]
        [BsonIgnoreIfNull]
        public GPS? GPS { get; set; }
    }

    public class GPS
    {
        [BsonElement("centre")]
        [JsonPropertyName("centre")]
        public double Centre { get; set; }

        [BsonElement("floor")]
        [JsonPropertyName("floor")]
        public int Floor { get; set; }
    }
}
