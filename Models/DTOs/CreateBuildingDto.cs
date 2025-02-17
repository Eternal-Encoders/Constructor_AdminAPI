using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using ConstructorAdminAPI.Models.Entities;

namespace ConstructorAdminAPI.Models.DTOs
{
    public class CreateBuildingDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("displayableName")]
        public string DisplayableName { get; set; }
        [JsonPropertyName("minFloor")]
        public int MinFloor { get; set; }
        [JsonPropertyName("maxFloor")]
        public int MaxFloor { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        //[JsonPropertyName("icon")]
        //public CreateIconDto? Icon { get; set; }
        [JsonPropertyName("gps")]
        public GPS? GPS { get; set; }
    }
}
