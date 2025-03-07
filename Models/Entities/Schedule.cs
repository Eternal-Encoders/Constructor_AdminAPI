using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Constructor_API.Models.Entities
{
    public class Schedule
    {
        [BsonElement("monday")]
        [JsonPropertyName("monday")]
        public Day? Monday { get; set; }

        [BsonElement("tuesday")]
        [JsonPropertyName("tuesday")]
        public Day? Tuesday { get; set; }

        [BsonElement("wednesday")]
        [JsonPropertyName("wednesday")]
        public Day? Wednesday { get; set; }

        [BsonElement("thursday")]
        [JsonPropertyName("thursday")]
        public Day? Thursday { get; set; }

        [BsonElement("friday")]
        [JsonPropertyName("friday")]
        public Day? Friday { get; set; }

        [BsonElement("saturday")]
        [JsonPropertyName("saturday")]
        public Day? Saturday { get; set; }

        [BsonElement("sunday")]
        [JsonPropertyName("sunday")]
        public Day? Sunday { get; set; }
    }

    public class Day
    {
        [BsonElement("isDayOff")]
        [JsonPropertyName("isDayOff")]
        [BsonIgnoreIfNull]
        public bool? IsDayOff { get; set; }

        [BsonElement("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [BsonElement("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }
}
}
