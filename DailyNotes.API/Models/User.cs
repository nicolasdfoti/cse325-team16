using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DailyNotes.API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }    
        public string PasswordHash { get; set; }
    }
}
