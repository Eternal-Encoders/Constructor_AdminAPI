using Constructor_API.Core.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Constructor_API.Helpers.Attributes;

namespace Constructor_API.Models.Entities
{
    public class Image : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("mime_type")]
        [BsonRequired]
        public string MimeType { get; set; }

        //[BsonElement("url")]
        //[JsonPropertyName("url")]
        //public string? Url { get; set; }

        [BsonElement("filesize")]
        [BsonRequired]
        public long Filesize { get; set; }

        [BsonElement("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("created_by")]
        public string? CreatedBy { get; set; }
    }
}
