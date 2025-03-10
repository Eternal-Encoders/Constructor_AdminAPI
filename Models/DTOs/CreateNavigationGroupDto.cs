using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs
{
    public class CreateNavigationGroupDto
    {
        [Required]
        [JsonRequired]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
