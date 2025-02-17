using ConstructorAdminAPI.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.Entities
{
    public class Room /*: Entity*/
    {
        [BsonElement("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }
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
        public string? Fill { get; set; }
        [BsonElement("stroke")]
        [JsonPropertyName("stroke")]
        public string? Stroke { get; set; }
        [BsonElement("pointId")]
        [JsonPropertyName("pointId")]
        public string PointId { get; set; }
        [BsonElement("children")]
        [JsonPropertyName("children")]
        [BsonIgnoreIfNull]
        public RoomChild[]? Children { get; set; }
        [BsonElement("doors")]
        [JsonPropertyName("doors")]
        [BsonIgnoreIfNull]
        public Door[]? Doors { get; set; }
    }

    public class RoomChild
    {
        private string type;
        [BsonElement("type")]
        [JsonPropertyName("type")]
        public string Type 
        {
            //get; set;
            get { return type; }
            set
            {
                if (value == "Icon" || value == "Text")
                    type = value;
            }
        }
        [BsonElement("identifier")]
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
        [BsonElement("x")]
        [JsonPropertyName("x")]
        public double X { get; set; }
        [BsonElement("y")]
        [JsonPropertyName("y")]
        public double Y { get; set; }
    }
}
