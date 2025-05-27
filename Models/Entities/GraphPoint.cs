using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    //[PointType]
    public class GraphPoint : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("x")]
        [BsonRequired]
        public double X { get; set; }

        [BsonElement("y")]
        [BsonRequired]
        public double Y { get; set; }

        [BsonElement("links")]
        [BsonRequired]
        public string[] Links { get; set; }

        [BsonElement("types")]
        [BsonRequired]
        public string[] Types { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("synonyms")]
        [BsonRequired]
        public string[] Synonyms { get; set; }

        [BsonElement("floor_id")]
        [BsonRequired]
        public string FloorId { get; set; }

        [BsonElement("time")]
        [BsonIgnoreIfNull]
        //public Schedule[]? Time { get; set; }
        public Day[]? Time { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        //[BsonElement("info")]
        //[JsonPropertyName("info")]
        //public InfoObject[]? Info { get; set; }

        [BsonElement("route_active")]
        [BsonRequired]
        public bool RouteActive { get; set; }

        [BsonElement("search_active")]
        [BsonRequired]
        public bool SearchActive { get; set; }

        [BsonElement("transition_id")]
        public string? TransitionId { get; set; }

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
