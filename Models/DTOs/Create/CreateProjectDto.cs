using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateProjectDto
    {
        [Required]
        [JsonPropertyName("name")]
        [MinLength(1)]
        [MaxLength(256)]
        public string? Name { get; set; }

        [DefaultValue("")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [DefaultValue("")]
        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        //[Required]
        //[JsonPropertyName("creator_id")]
        //public string? CreatorId { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
