using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Entities
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string OwnerId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public long Timestamp { get; set; }
    }
}