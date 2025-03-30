using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateUserDto
    {
        [JsonPropertyName("nickname")]
        [Required]
        public string? Nickname { get; set; }

        [JsonPropertyName("email")]
        [Required]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        [Required]
        public string? Password { get; set; }
    }
}
