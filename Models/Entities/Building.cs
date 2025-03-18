using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    [MinMaxFloorValidation]
    public class Building : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("project_id")]
        [JsonPropertyName("project_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProjectId { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("displayable_name")]
        [JsonPropertyName("displayable_name")]
        public string DisplayableName { get; set; }

        [BsonElement("min_floor")]
        [JsonPropertyName("min_floor")]
        public int MinFloor { get; set; }

        [BsonElement("max_floor")]
        [JsonPropertyName("max_floor")]
        public int MaxFloor { get; set; }

        [BsonElement("floors")]
        [JsonPropertyName("floors")]
        public string[]? FloorIds { get; set; }

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
        [StringLength(24)]
        public string? Icon { get; set; }

        [BsonElement("gps")]
        [JsonPropertyName("gps")]
        [BsonIgnoreIfNull]
        public GPS? GPS { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class GPS
    {
        [BsonElement("centre")]
        [JsonPropertyName("centre")]
        [Required]
        public double Centre { get; set; }

        [BsonElement("floor")]
        [JsonPropertyName("floor")]
        [Required]
        public int Floor { get; set; }
    }
}
