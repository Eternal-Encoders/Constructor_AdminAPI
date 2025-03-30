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
        public string? Password { get; set; }
    }
}
