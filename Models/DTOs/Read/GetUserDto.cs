using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Read
{
    public class GetUserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nickname")]
        public string? Nickname { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("paid_feature_id")]
        public string[]? FeatureIds { get; set; }

        [JsonPropertyName("selected_project_id")]
        public string SelectedProject { get; set; }
    }
}
