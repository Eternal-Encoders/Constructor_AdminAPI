using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.InnerObjects;
using Constructor_API.Models.Entities;

namespace Constructor_API.Models.DTOs.Create
{
    [PointType]
    public class CreateGraphPointFromFloorDto
    {
        //private static readonly Dictionary<string, GraphPointType> pointTypes = new Dictionary<string, GraphPointType>()
        //{
        //    ["corridor"] = new GraphPointType { "corridor" },
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

        [JsonPropertyName("id")]
        [Required]
        [ObjectId]
        public string? Id { get; set; }

        [JsonPropertyName("x")]
        [Required]
        public double? X { get; set; }

        [JsonPropertyName("y")]
        [Required]
        public double? Y { get; set; }

        [JsonPropertyName("links")]
        [Required]
        public string[]? Links { get; set; }

        //private string[]? types;
        [JsonPropertyName("types")]
        [Required]
        public string[]? Types { get; set; }
        //{
        //    get { return types; }
        //    set
        //    {
        //        types = value;
        //        types = types.Where(x => pointTypes.ContainsKey(x)).ToArray();
        //    }
        //}

        //[JsonPropertyName("types")]
        //[Required]
        //public GraphPointType[]? Types { get; set; }

        [JsonPropertyName("names")]
        [Required]
        public string[]? Names { get; set; }

        //[JsonPropertyName("floor")]
        //[JsonRequired]
        //[Required]
        //public int Floor { get; set; }

        //[JsonPropertyName("building")]
        //[JsonRequired]
        //[Required]
        //public string Building { get; set; }

        [JsonPropertyName("time")]
        //[BsonIgnoreIfNull]
        //public Schedule[]? Time { get; set; }
        public Day[]? Time { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("info")]
        public string? Info { get; set; }

        [JsonPropertyName("is_pass_free")]
        [Required]
        public bool? IsPassFree { get; set; }

        [ObjectId]
        [JsonPropertyName("connection_id")]
        public string? ConnectionId { get; set; }
    }
}
