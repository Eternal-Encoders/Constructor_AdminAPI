using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Models.DTOs;

namespace Constructor_API.Models.InnerObjects
{
    public class GraphPointType
    {
        [Required]
        [BsonRequired]
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [BsonElement("category")]
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [BsonElement("image_id")]
        [JsonPropertyName("image_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ImageId { get; set; }

        [BsonRequired]
        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonRequired]
        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        [JsonPropertyName("updated_by")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UpdatedBy { get; set; }
    }
}
