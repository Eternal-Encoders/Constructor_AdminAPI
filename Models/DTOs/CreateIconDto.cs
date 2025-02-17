using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.DTOs
{
    public class CreateIconDto
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("alt")]
        public string Alt { get; set; }
    }
}
