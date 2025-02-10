using ConstructorAdminAPI.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace ConstructorAdminAPI.Models.Entities
{
    public class InstituteFloor : MongoRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("floorNumber")]
        [JsonPropertyName("floorNumber")]
        public int FloorNumber { get; set; }
        [BsonElement("width")]
        [JsonPropertyName("width")]
        public int Width { get; set; }
        [BsonElement("height")]
        [JsonPropertyName("height")]
        public int Height { get; set; }
        [BsonElement("service")]
        [JsonPropertyName("service")]
        public Service[] Service { get; set; }
        [BsonElement("audiences")]
        [JsonPropertyName("audiences")]
        public Room[] Audiences { get; set; }
        [BsonElement("graph")]
        [JsonPropertyName("graph")]
        public string[] GraphPoints { get; set; }
    }
}
