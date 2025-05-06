using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Models.DTOs.Create
{
    public class CreateUserDto
    {
        [JsonPropertyName("nickname")]
        [MinLength(2)]
        [MaxLength(100)]
        //[Required]
        public string? Nickname { get; set; }

        [MinLength(6)]
        [MaxLength(256)]
        [JsonPropertyName("email")]
        [Required]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        [Required]
        [MinLength(8)]
        [MaxLength(32)]
        public string? Password { get; set; }
    }
}
