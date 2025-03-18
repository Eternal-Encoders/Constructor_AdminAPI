using Constructor_API.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Driver.GeoJsonObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Constructor_API.Models.Entities
{
    public class Floor : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("floor_number")]
        [JsonPropertyName("floor_number")]
        public int FloorNumber { get; set; }

        [BsonElement("building_id")]
        [JsonPropertyName("building_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BuildingId { get; set; }

        [BsonElement("building")]
        [JsonPropertyName("building")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Building { get; set; }

        [BsonElement("width")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [BsonElement("height")]
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [BsonElement("service")]
        [JsonPropertyName("service")]
        public Service[] Service { get; set; }

        [BsonElement("rooms")]
        [JsonPropertyName("rooms")]
        public Room[] Rooms { get; set; }

        [BsonElement("graph")]
        [JsonPropertyName("graph")]
        public string[] GraphPoints { get; set; }

        [BsonElement("forces")]
        [JsonPropertyName("forces")]
        [BsonIgnoreIfNull]
        public Forces[]? Forces { get; set; }
    }

    public class Service
    {
        [BsonElement("x")]
        [JsonPropertyName("x")]
        [Required]
        public double X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [Required]
        public double Y { get; set; }

        [BsonElement("data")]
        [JsonPropertyName("data")]
        public string? Data { get; set; }

        [BsonElement("stroke")]
        [JsonPropertyName("stroke")]
        [BsonIgnoreIfNull]
        public string? Stroke { get; set; }

        [BsonElement("fill")]
        [JsonPropertyName("fill")]
        [BsonIgnoreIfNull]
        public string? Fill { get; set; }
    }

    public class Forces
    {
        [BsonElement("point")]
        [JsonPropertyName("point")]
        [Required]
        public Coordinates Point {  get; set; }

        [BsonElement("force")]
        [JsonPropertyName("force")]
        [Required]
        public Coordinates Force { get; set; }
    }

    public class Coordinates
    {
        [BsonElement("x")]
        [JsonPropertyName("x")]
        [Required]
        public double X { get; set; }

        [BsonElement("y")]
        [JsonPropertyName("y")]
        [Required]
        public double Y { get; set; }
    }
}
