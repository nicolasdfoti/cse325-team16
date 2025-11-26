using DailyNotes.API.Models;
using MongoDB.Driver;

namespace DailyNotes.API.Services
{
    public class EntryDbService
    {
        private readonly IMongoCollection<Entry> _entries;

        public EntryDbService(IMongoDatabase db)
        {
            _entries = db.GetCollection<Entry>("Entries");
        }

        // === GETS ===

        public async Task<List<Entry>> GetEntriesByUserIdAsync(string userId)
        {
            return await _entries
                .Find(e => e.UserId == userId)
                .SortByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<Entry?> GetByIdAsync(string id)
        {
            return await _entries
                .Find(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Entry>> GetPublicEntriesAsync()
        {
            return await _entries
                .Find(_ => true)
                .SortByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        // === CRUD ===

        public async Task CreateAsync(Entry entry)
        {
            entry.CreatedAt = DateTime.UtcNow;
            await _entries.InsertOneAsync(entry);
        }

        public async Task UpdateAsync(Entry entry)
        {
            await _entries.ReplaceOneAsync(
                e => e.Id == entry.Id && e.UserId == entry.UserId,
                entry
            );
        }

        public async Task DeleteAsync(string id, string userId)
        {
            await _entries.DeleteOneAsync(
                e => e.Id == id && e.UserId == userId
            );
        }

        // === Likes / Dislikes ===

        public async Task LikeAsync(string id)
        {
            var update = Builders<Entry>.Update.Inc(e => e.Likes, 1);
            await _entries.UpdateOneAsync(e => e.Id == id, update);
        }

        public async Task DislikeAsync(string id)
        {
            var update = Builders<Entry>.Update.Inc(e => e.Dislikes, 1);
            await _entries.UpdateOneAsync(e => e.Id == id, update);
        }

        // === Comments ===

        public async Task AddCommentAsync(string entryId, string author, string text)
        {
            var comment = new Comment
            {
                Author = author,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            var update = Builders<Entry>
                .Update.Push(e => e.Comments, comment);

            await _entries.UpdateOneAsync(e => e.Id == entryId, update);
        }
    }
}
