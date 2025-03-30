using Constructor_API.Helpers.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.InnerObjects
{
    public class ProjectUser
    {
        [BsonElement("user_id")]
        [JsonPropertyName("user_id")]
        [ObjectId]
        [BsonRequired]
        public string UserId { get; set; }

        [BsonElement("project_role")]
        [JsonPropertyName("project_role")]
        [BsonRequired]
        public string ProjectRole { get; set; }

        [BsonElement("added_at")]
        [JsonPropertyName("added_at")]
        [BsonRequired]
        public DateTime AddedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }
    }
}
