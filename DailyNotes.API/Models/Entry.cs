using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DailyNotes.API.Models
{
    public class Entry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;

        public bool IsPublic { get; set; } = false;
        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;

        public List<Comment> Comments { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
