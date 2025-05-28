using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Constructor_API.Core.Shared;

namespace Constructor_API.Models.Entities
{
    public class Icon : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("filename")]
        public string Filename { get; set; }

        [BsonElement("mime_type")]
        public string? MimeType { get; set; }
    }
}
