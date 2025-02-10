using ConstructorAdminAPI.Models.Entities;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.DTOs
{
    public class CreateFloorDto
    {
        [JsonPropertyName("floor")]
        public int FloorNumber { get; set; }
        [JsonPropertyName("building")]
        public string Building { get; set; }
        [JsonPropertyName("width")]
        public int Width { get; set; }
        [JsonPropertyName("height")]
        public int Height { get; set; }
        [JsonPropertyName("service")]
        public Service[] Service { get; set; }
        [JsonPropertyName("rooms")]
        public Dictionary<string, Room>? Rooms { get; set; }
        [JsonPropertyName("graph")]
        public Dictionary<string, GraphPoint>? GraphPoints { get; set; }
        [JsonPropertyName("forces")]
        public Forces[]? Forces { get; set; }
    }
}
