using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class Building : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("project_id")]
        [BsonRequired]
        public string ProjectId { get; set; }

        //[BsonIgnore]
        //[JsonPropertyName("project")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public Project Project { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("displayable_name")]
        [BsonRequired]
        public string DisplayableName { get; set; }

        [BsonElement("floor_ids")]
        [BsonRequired]
        public string[]? FloorIds { get; set; }

        //[BsonIgnore]
        //[JsonPropertyName("floors")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public Floor[]? Floors { get; set; }

        [BsonElement("url")]
        [BsonRequired]
        public string Url { get; set; }

        [BsonElement("latitude")]
        [BsonRequired]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        [BsonRequired]
        public double Longitude { get; set; }

        [BsonElement("image_id")]
        public string? ImageId { get; set; }

        [BsonElement("gps")]
        [BsonIgnoreIfNull]
        public GPS? GPS { get; set; }

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
