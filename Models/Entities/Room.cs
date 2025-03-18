using Constructor_API.Core.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class Room 
    {
        //[BsonId]
        //[BsonElement("_id")]
        //[JsonPropertyName("id")]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }
        [BsonElement("x")]
        [JsonPropertyName("x")]
        [JsonRequired]
        [Required]
        public double X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [JsonRequired]
        [Required]
        public double Y { get; set; }

        [BsonElement("width")]
        [JsonPropertyName("width")]
        [JsonRequired]
        [Required]
        public double Width { get; set; }

        [BsonElement("height")]
        [JsonPropertyName("height")]
        [JsonRequired]
        [Required]
        public double Height { get; set; }

        [BsonElement("fill")]
        [JsonPropertyName("fill")]
        [Required]
        public string? Fill { get; set; }

        [BsonElement("stroke")]
        [JsonPropertyName("stroke")]
        [Required]
        public string? Stroke { get; set; }

        //[BsonElement("pointId")]
        //[JsonPropertyName("pointId")]
        //public string PointId { get; set; }
        [BsonElement("children")]
        [JsonPropertyName("children")]
        [BsonIgnoreIfNull]
        [Required]
        public RoomChild[]? Children { get; set; }

        [BsonElement("doors")]
        [JsonPropertyName("doors")]
        [BsonIgnoreIfNull]
        [Required]
        public Door[]? Doors { get; set; }
    }

    public class RoomChild
    {
        private string type;
        [BsonElement("type")]
        [JsonPropertyName("type")]
        [JsonRequired]
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
        [JsonRequired]
        public string Identifier { get; set; }

        [BsonElement("x")]
        [JsonPropertyName("x")]
        [JsonRequired]
        public double X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [JsonRequired]
        public double Y { get; set; }
    }
}
