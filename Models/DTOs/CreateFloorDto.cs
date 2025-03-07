using Constructor_API.Models.Entities;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs
{
    public class CreateFloorDto
    {
        [JsonPropertyName("floor")]
        [JsonRequired]
        public int FloorNumber { get; set; }

        [JsonPropertyName("building")]
        [JsonRequired]
        public string Building { get; set; }

        [JsonPropertyName("width")]
        [JsonRequired]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        [JsonRequired]
        public int Height { get; set; }

        [JsonPropertyName("service")]
        [JsonRequired]
        public Service[] Service { get; set; }

        [JsonPropertyName("graph")]
        public Dictionary<string, GraphPoint>? GraphPoints { get; set; }

        [JsonPropertyName("forces")]
        public Forces[]? Forces { get; set; }
    }
}
