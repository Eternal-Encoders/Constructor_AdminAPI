﻿using Constructor_API.Core.Shared;
using Constructor_API.Helpers.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class FloorsTransition : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        [ObjectId]
        [Required]
        public string? Id { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        [Required]
        public string? Name { get; set; }

        [BsonElement("building_id")]
        [JsonPropertyName("building_id")]
        [Required]
        [ObjectId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? BuildingId { get; set; }

        //[BsonIgnore]
        //public Building? Building { get; set; }

        [BsonElement("links")]
        [JsonPropertyName("links")]
        [Required]
        [ObjectId]
        public string[]? LinkIds { get; set; }

        [BsonElement("direction")]
        [JsonPropertyName("direction")]
        public string? Direction { get; set; }

        //[BsonIgnore]
        //[JsonPropertyName("links")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public GraphPoint[]? Links { get; set; }

        [BsonElement("created_at")]
        [JsonPropertyName("created_at")]
        [Required]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [JsonPropertyName("updated_at")]
        [Required]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("updated_by")]
        [JsonPropertyName("updated_by")]
        [ObjectId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UpdatedBy { get; set; }
    }
}
