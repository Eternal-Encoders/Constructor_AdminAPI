using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Objects
{
    public class RoomMarker
    {
        private string type;
        [BsonElement("type")]
        [JsonPropertyName("type")]
        [Required]
        [BsonRequired]
        public string? Type
        {
            //get; set;
            get { return type; }
            set
            {
                if (value == "Icon" || value == "Text")
                    type = value;
            }
        }

        [BsonElement("value")]
        [JsonPropertyName("value")]
        [Required]
        [BsonRequired]
        public string? Value { get; set; }

        [BsonElement("x")]
        [JsonPropertyName("x")]
        [Required]
        [BsonRequired]
        public double? X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [Required]
        [BsonRequired]
        public double? Y { get; set; }
    }
}
