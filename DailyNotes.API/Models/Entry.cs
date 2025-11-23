using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DailyNotes.API.Models
{
    public class Entry
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public bool IsPublic { get; set; }
        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;
        public List<Comment> Comments { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
