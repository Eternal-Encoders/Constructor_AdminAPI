using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Constructor_API.Models.DTOs.Read
{
    public class FloorForPathDto
    {
        [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("index")]
        public int Index { get; set; }

        [BsonElement("graph")]
        public GraphPoint[]? GraphPoints { get; set; }

        [BsonElement("building_id")]
        public string BuildingId { get; set; }
    }
}
