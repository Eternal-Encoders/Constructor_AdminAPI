using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.InnerObjects
{
    public class GPS
    {
        [BsonElement("centre")]
        [JsonPropertyName("centre")]
        [Required]
        [BsonRequired]
        public double? Centre { get; set; }

        [BsonElement("floor")]
        [JsonPropertyName("floor")]
        [Required]
        [BsonRequired]
        public int? Floor { get; set; }
    }
}
