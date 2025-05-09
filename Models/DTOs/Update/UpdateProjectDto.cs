using Constructor_API.Helpers.Attributes;
using Constructor_API.Models.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.DTOs.Update
{
    public class UpdateProjectDto
    {
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("description")]
        [BsonElement("description")]
        public string? Description { get; set; }

        //[ObjectId]
        //[JsonPropertyName("image_id")]
        //[BsonElement("image_id")]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string? ImageId { get; set; }

        [BsonElement("custom_graph_point_types")]
        [JsonPropertyName("custom_graph_point_types")]
        [BsonIgnoreIfNull]
        public GraphPointType[]? CustomGraphPointTypes { get; set; }
    }
}
