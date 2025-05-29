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
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("url")]
        [BsonRequired]
        public string Url { get; set; }

        [BsonElement("building_ids")]
        [BsonRequired]
        public string[] BuildingIds { get; set; }

        //[BsonElement("project_users")]
        //[JsonPropertyName("project_users")]
        //[BsonRequired]
        //public string[] ProjectUserIds { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("icon")]
        public string? Icon { get; set; }

        [BsonElement("image_id")]
        public string? ImageId { get; set; }

        [BsonElement("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("custom_graph_point_types")]
        [BsonIgnoreIfNull]
        public GraphPointType[]? CustomGraphPointTypes { get; set; }
    }
}
