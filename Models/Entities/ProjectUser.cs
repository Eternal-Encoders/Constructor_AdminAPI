using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class ProjectUser : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user_id")]
        [BsonRequired]
        public string UserId { get; set; }

        [BsonElement("project_id")]
        [BsonRequired]
        public string ProjectId { get; set; }

        [BsonElement("project_role")]
        [JsonPropertyName("project_role")]
        [BsonRequired]
        public string ProjectRole { get; set; }

        [BsonElement("added_at")]
        [BsonRequired]
        public DateTime AddedAt { get; set; }

        [BsonElement("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }
    }
}
