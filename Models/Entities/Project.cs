using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Constructor_API.Core.Shared;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Models.Objects;
using Constructor_API.Helpers.Attributes;

namespace Constructor_API.Models.Entities
{
    public class Project: IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("url")]
        [JsonPropertyName("url")]
        [BsonRequired]
        public string Url { get; set; }

        [BsonElement("buildings")]
        [JsonPropertyName("buildings")]
        [BsonRequired]
        public string[] BuildingIds { get; set; }

        //[BsonElement("project_users")]
        //[JsonPropertyName("project_users")]
        //[BsonRequired]
        //public string[] ProjectUserIds { get; set; }

        [BsonIgnore]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProjectUser[]? ProjectUsers { get; set; }

        [JsonPropertyName("description")]
        [BsonElement("description")]
        public string? Description { get; set; }

        //[ObjectId]
        //[JsonPropertyName("image_id")]
        //[BsonElement("image_id")]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string? ImageId { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("custom_graph_point_types")]
        [JsonPropertyName("custom_graph_point_types")]
        [BsonIgnoreIfNull]
        public GraphPointType[]? CustomGraphPointTypes { get; set; }
    }
}
