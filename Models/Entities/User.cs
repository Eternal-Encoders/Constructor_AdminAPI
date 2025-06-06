﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Constructor_API.Core.Shared;
using System.Text;
using System.Security.Cryptography;

namespace Constructor_API.Models.Entities
{
    public class User : IAggregateRoot
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nickname")]
        //[BsonRequired]
        public string? Nickname { get; set; }

        [BsonElement("email")]
        [BsonRequired]
        public string Email { get; set; }

        [BsonElement("password_hash")]
        [BsonRequired]
        public string PasswordHash { get; set; }

        //[BsonElement("project_users")]
        //[JsonPropertyName("project_users")]
        //public string[] ProjectUserIds { get; set; }

        //[BsonElement("project_users")]
        //[JsonPropertyName("project_users")]
        //[BsonIgnoreIfNull]
        //public ProjectUser[]? ProjectUsers { get; set; }

        [BsonElement("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        [BsonRequired]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("paid_feature_id")]
        [BsonIgnoreIfNull]
        public string[]? FeatureIds { get; set; }

        [BsonElement("selected_project_id")]
        public string SelectedProject { get; set; }

        [JsonPropertyName("last_building_id")]
        public string LastBuilding { get; set; }
    }
}
