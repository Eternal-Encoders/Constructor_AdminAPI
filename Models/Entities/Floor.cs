using Constructor_API.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Driver.GeoJsonObjectModel;
using System.ComponentModel.DataAnnotations;
using Constructor_API.Models.Objects;
using Constructor_API.Helpers.Attributes;

namespace Constructor_API.Models.Entities
{
    public class Floor : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("index")]
        [BsonRequired]
        public int Index { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("building_id")]
        [BsonRequired]
        public string BuildingId { get; set; }

        //[BsonIgnore]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public Building? Building { get; set; }

        //[BsonElement("image_ids")]
        //[JsonPropertyName("image_ids")]
        //[ObjectId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string[] ImageIds { get; set; }

        [BsonElement("background")]
        [BsonIgnoreIfNull]
        public BackgroundImage? Background { get; set; }

        [BsonElement("width")]
        [BsonRequired]
        public int Width { get; set; }

        [BsonElement("height")]
        [BsonRequired]
        public int Height { get; set; }

        [BsonElement("decorations")]
        [BsonRequired]
        public Decoration[] Decorations { get; set; }

        [BsonElement("rooms")]
        [BsonRequired]
        public Room[] Rooms { get; set; }

        [BsonElement("graph")]
        [BsonRequired]
        public string[] GraphPoints { get; set; }

        [BsonElement("forces")]
        [BsonIgnoreIfNull]
        public Forces[]? Forces { get; set; }

        [BsonElement("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        public string? UpdatedBy { get; set; }
    }
}
