using Constructor_API.Models.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Helpers.Attributes;

namespace Constructor_API.Models.DTOs
{
    [PointType]
    public class GraphPointFromFloorDto
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
            ["escalator"] = "escalator",
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

        [JsonPropertyName("id")]
        [JsonRequired]
        [Required]
        [StringLength(24)]
        public string Id { get; set; }

        [JsonPropertyName("x")]
        [JsonRequired]
        [Required]
        public double X { get; set; }

        [JsonPropertyName("y")]
        [JsonRequired]
        [Required]
        public double Y { get; set; }

        [JsonPropertyName("links")]
        [JsonRequired]
        [Required]
        public string[] Links { get; set; }

        private string[] types;
        [JsonPropertyName("types")]
        [JsonRequired]
        [Required]
        public string[] Types
        {
            get { return types; }
            set
            {
                types = value;
                types = types.Where(x => pointTypes.ContainsKey(x)).ToArray();
            }
        }

        [JsonPropertyName("names")]
        [JsonRequired]
        [Required]
        public string[] Names { get; set; }

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
        [JsonRequired]
        [Required]
        public bool IsPassFree { get; set; }

        [StringLength(24)]
        [JsonPropertyName("stair_id")]
        public string? StairId { get; set; }

        [JsonPropertyName("room")]
        public Room? Room { get; set; }
    }
}
