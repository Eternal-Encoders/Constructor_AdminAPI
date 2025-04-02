using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs
{
    public class LoginUserDto
    {
        [Required]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
