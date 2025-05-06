using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Constructor_API.Models.DTOs.Read
{
    public class FloorForPathDto
    {
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [ObjectId]
        public string Id { get; set; }

        [BsonElement("number")]
        public int FloorNumber { get; set; }

        [BsonElement("graph")]
        public GraphPoint[]? GraphPoints { get; set; }
    }
}
