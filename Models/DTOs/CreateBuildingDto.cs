using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Constructor_API.Models.Entities;

namespace Constructor_API.Models.DTOs
{
    public class CreateBuildingDto
    {
        [JsonPropertyName("name")]
        [JsonRequired]
        public string Name { get; set; }

        [JsonPropertyName("displayableName")]
        [JsonRequired]
        public string DisplayableName { get; set; }

        [JsonPropertyName("minFloor")]
        [JsonRequired]
        public int MinFloor { get; set; }

        [JsonPropertyName("maxFloor")]
        [JsonRequired]
        public int MaxFloor { get; set; }

        [JsonPropertyName("url")]
        [JsonRequired]
        public string Url { get; set; }

        [JsonPropertyName("latitude")]
        [JsonRequired]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        [JsonRequired]
        public double Longitude { get; set; }

        //[JsonPropertyName("icon")]
        //public CreateIconDto? Icon { get; set; }
        [JsonPropertyName("gps")]
        public GPS? GPS { get; set; }
    }
}
