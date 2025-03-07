using Constructor_API.Core.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class GraphPoint : IAggregateRoot
    {
        private static readonly Dictionary<string, string> pointTypes = new Dictionary<string, string>()
        {
            ["corridor"] = "corridor",
            //["auditorium"] = "auditorium",
            //["dinning"] = "dinning",
            ["exit"] = "exit",
            ["fire-exit"] = "fire-exit",
            ["stair"] = "stair",
            ["elevator"] = "elevator",
            ["toilet-m"] = "toilet-m",
            ["toilet-w"] = "toilet-w",
            ["cafe"] = "cafe",
            //["vending"] = "vending",
            //["coworking"] = "coworking",
            //["atm"] = "atm",
            ["wardrobe"] = "wardrobe",
            //["print"] = "print",
            //["deanery"] = "deanery",
            //["students"] = "students",
            ["other"] = "other"
        };

        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonRequired]
        public string Id { get; set; }

        [BsonElement("x")]
        [JsonPropertyName("x")]
        [JsonRequired]
        [BsonRequired]
        public double X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [JsonRequired]
        [BsonRequired]
        public double Y { get; set; }

        [BsonElement("links")]
        [JsonPropertyName("links")]
        [JsonRequired]
        [BsonRequired]
        public string[] Links { get; set; }

        private string[] types;
        [BsonElement("types")]
        [JsonPropertyName("types")]
        [JsonRequired]
        [BsonRequired]
        public string[] Types {
            get { return types; }
            set 
            {
                types = value;
                types = types.Where(x => pointTypes.ContainsKey(x)).ToArray();
            }
        } 

        [BsonElement("names")]
        [JsonPropertyName("names")]
        [JsonRequired]
        [BsonRequired]
        public string[] Names { get; set; }

        [BsonElement("floor")]
        [JsonPropertyName("floor")]
        [JsonRequired]
        [BsonRequired]
        public int Floor { get; set; }

        [BsonElement("building")]
        [JsonPropertyName("building")]
        [JsonRequired]
        [BsonRequired]
        public string Building { get; set; }

        [BsonElement("time")]
        [JsonPropertyName("time")]
        //[BsonIgnoreIfNull]
        //public Schedule[]? Time { get; set; }
        public Day[]? Time { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        [JsonRequired]
        [BsonRequired]
        public string Description { get; set; }

        [BsonElement("info")]
        [JsonPropertyName("info")]
        [JsonRequired]
        [BsonRequired]
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

        [BsonElement("room")]
        [JsonPropertyName("room")]
        [BsonIgnoreIfNull]
        public Room? Room { get; set; }
    }
}
