using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DailyNotes.API.Models
{
    public class JournalEntry
    {
        [BsonId]
        public ObjectId JournalEntryId { get; set; }
        public ObjectId UserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Prompt { get; set; }
    }
}
