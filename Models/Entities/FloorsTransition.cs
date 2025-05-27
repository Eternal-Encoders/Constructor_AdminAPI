using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class FloorsTransition : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string? Name { get; set; }

        [BsonElement("building_id")]
        [BsonRequired]
        public string? BuildingId { get; set; }

        //[BsonIgnore]
        //public Building? Building { get; set; }

        [BsonElement("links")]
        [BsonRequired]
        public string[] LinkIds { get; set; }

        [BsonElement("direction")]
        public string? Direction { get; set; }

        //[BsonIgnore]
        //[JsonPropertyName("links")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public GraphPoint[]? Links { get; set; }

        [BsonElement("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        public string? UpdatedBy { get; set; }
    }
}
