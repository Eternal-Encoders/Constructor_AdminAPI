using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.InnerObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    [PointType]
    public class GraphPoint : IAggregateRoot
    {
        //private static readonly Dictionary<string, string> pointTypes = new Dictionary<string, string>()
        //{
        //    ["corridor"] = "corridor",
        //    ["exit"] = "exit",
        //    ["fire-exit"] = "fire-exit",
        //    ["stair"] = "stair",
        //    ["elevator"] = "elevator",
        //    ["escalator"] = "escalator",
        //    ["toilet-m"] = "toilet-m",
        //    ["toilet-w"] = "toilet-w",
        //    ["cafe"] = "cafe",
        //    ["dinning"] = "dinning",
        //    ["restaurant"] = "restaurant",
        //    ["wardrobe"] = "wardrobe",
        //    ["other"] = "other"
        //};

        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [ObjectId]
        public string Id { get; set; }

        [BsonElement("x")]
        [JsonPropertyName("x")]
        [BsonRequired]
        public double X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [BsonRequired]
        public double Y { get; set; }

        [BsonElement("links")]
        [JsonPropertyName("links")]
        [BsonRequired]
        public string[] Links { get; set; }

        //private string[]? types;
        [BsonElement("types")]
        [JsonPropertyName("types")]
        [BsonRequired]
        public string[] Types { get; set; }
        //{
        //    get { return types; }
        //    set 
        //    {
        //        types = value;
        //        types = types.Where(x => pointTypes.ContainsKey(x)).ToArray();
        //    }
        //} 

        [BsonElement("names")]
        [JsonPropertyName("names")]
        [BsonRequired]
        public string[] Names { get; set; }

        [BsonElement("floor")]
        [JsonPropertyName("floor")]
        [ObjectId]
        [BsonRequired]
        public string FloorId { get; set; }

        //[BsonElement("building")]
        //[JsonPropertyName("building")]
        //[BsonRequired]
        //public string Building { get; set; }

        [BsonElement("time")]
        [JsonPropertyName("time")]
        [BsonIgnoreIfNull]
        //public Schedule[]? Time { get; set; }
        public Day[]? Time { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [BsonElement("info")]
        [JsonPropertyName("info")]
        public string? Info { get; set; }

        [BsonElement("is_pass_free")]
        [JsonPropertyName("is_pass_free")]
        [BsonRequired]
        public bool IsPassFree { get; set; }

        [BsonElement("connection_id")]
        [JsonPropertyName("connection_id")]
        [BsonIgnoreIfNull]
        public string? ConnectionId { get; set; }

        //[BsonElement("room_id")]
        //[JsonPropertyName("room_id")]
        //[ObjectId]
        //public string? RoomId { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        [JsonPropertyName("updated_by")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UpdatedBy { get; set; }
    }
}
