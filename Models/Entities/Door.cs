using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.Entities
{
    public class Door
    {
        [BsonElement("x")]
        [JsonPropertyName("x")]
        public double X { get; set; }
        [BsonElement("y")]
        [JsonPropertyName("y")]
        public double Y { get; set; }
        [BsonElement("width")]
        [JsonPropertyName("width")]
        public double Width { get; set; }
        [BsonElement("height")]
        [JsonPropertyName("height")]
        public double Height { get; set; }
        [BsonElement("fill")]
        [JsonPropertyName("fill")]
        public string Fill { get; set; }
    }
}
