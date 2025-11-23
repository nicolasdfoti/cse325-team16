using DailyNotes.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DailyNotes.API.Services
{
    public class EntryService
    {
        private readonly IMongoCollection<Entry> _entries;

        public EntryService(IMongoDatabase database)
        {
            _entries = database.GetCollection<Entry>("Entries");
        }

        // ------------------------------
        // CREATE
        // ------------------------------
        public async Task CreateAsync(Entry entry)
        {
            entry.Id = ObjectId.GenerateNewId();
            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;

            if (entry.Comments == null)
                entry.Comments = new List<Comment>();

            await _entries.InsertOneAsync(entry);
        }

        // ------------------------------
        // GET ALL (MY ENTRIES)
        // ------------------------------
        public async Task<List<Entry>> GetMyEntriesAsync()
        {
            return await _entries.Find(_ => true)
                                 .SortByDescending(e => e.CreatedAt)
                                 .ToListAsync();
        }

        // ------------------------------
        // GET PUBLIC
        // ------------------------------
        public async Task<List<Entry>> GetPublicEntriesAsync()
        {
            return await _entries.Find(e => e.IsPublic)
                                 .SortByDescending(e => e.CreatedAt)
                                 .ToListAsync();
        }

        // ------------------------------
        // GET BY ID
        // ------------------------------
        public async Task<Entry?> GetEntryByIdAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            return await _entries.Find(e => e.Id == objectId).FirstOrDefaultAsync();
        }

        // ------------------------------
        // DELETE
        // ------------------------------
        public async Task DeleteEntryAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _entries.DeleteOneAsync(e => e.Id == objectId);
        }

        // ------------------------------
        // UPDATE
        // ------------------------------
        public async Task UpdateEntryAsync(Entry updated)
        {
            updated.UpdatedAt = DateTime.Now;
            await _entries.ReplaceOneAsync(e => e.Id == updated.Id, updated);
        }

        // ------------------------------
        // LIKE
        // ------------------------------
        public async Task LikeEntryAsync(string id)
        {
            var objectId = ObjectId.Parse(id);

            var update = Builders<Entry>.Update.Inc(e => e.Likes, 1);

            await _entries.UpdateOneAsync(e => e.Id == objectId, update);
        }

        // ------------------------------
        // DISLIKE
        // ------------------------------
        public async Task DislikeEntryAsync(string id)
        {
            var objectId = ObjectId.Parse(id);

            var update = Builders<Entry>.Update.Inc(e => e.Dislikes, 1);

            await _entries.UpdateOneAsync(e => e.Id == objectId, update);
        }

        // ------------------------------
        // ADD COMMENT
        // ------------------------------
        public async Task AddCommentAsync(string entryId, string author, string text)
        {
            var objectId = ObjectId.Parse(entryId);

            var comment = new Comment
            {
                Author = author,
                Text = text,
                CreatedAt = DateTime.Now
            };

            var update = Builders<Entry>.Update.Push(e => e.Comments, comment);

            await _entries.UpdateOneAsync(e => e.Id == objectId, update);
        }
    }
}
