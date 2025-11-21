using DailyNotes.Models;

namespace DailyNotes.Services
{
    public class EntryService
    {
        private readonly List<Entry> _entries = new();
        private int _idCounter = 1;

        // ------------------------------
        // CREATE NEW ENTRY
        // ------------------------------
        public Task SaveEntryAsync(Entry entry)
        {
            entry.Id = _idCounter++;
            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;
            entry.Comments = new List<Comment>();
            _entries.Add(entry);

            return Task.CompletedTask;
        }

        // ------------------------------
        // GET ENTRIES (MY ENTRIES)
        // ------------------------------
        public Task<List<Entry>> GetMyEntriesAsync()
        {
            return Task.FromResult(
                _entries.OrderByDescending(e => e.CreatedAt).ToList()
            );
        }

        // ------------------------------
        // GET PUBLIC ENTRIES
        // ------------------------------
        public Task<List<Entry>> GetPublicEntriesAsync()
        {
            return Task.FromResult(
                _entries.Where(e => e.IsPublic)
                        .OrderByDescending(e => e.CreatedAt)
                        .ToList()
            );
        }

        // ------------------------------
        // GET ENTRY BY ID
        // ------------------------------
        public Task<Entry?> GetEntryByIdAsync(int id)
        {
            return Task.FromResult(
                _entries.FirstOrDefault(e => e.Id == id)
            );
        }

        // ------------------------------
        // DELETE
        // ------------------------------
        public Task DeleteEntryAsync(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry != null)
                _entries.Remove(entry);

            return Task.CompletedTask;
        }

        // ------------------------------
        // UPDATE (EDIT ENTRY)
        // ------------------------------
        public Task UpdateEntryAsync(Entry updated)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == updated.Id);
            if (entry != null)
            {
                entry.Title = updated.Title;
                entry.Content = updated.Content;
                entry.IsPublic = updated.IsPublic;

                entry.UpdatedAt = DateTime.Now;

                if (entry.Comments == null)
                    entry.Comments = new List<Comment>();
            }
            return Task.CompletedTask;
        }

        // ------------------------------
        // LIKE
        // ------------------------------
        public Task LikeEntryAsync(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry != null)
                entry.Likes++;

            return Task.CompletedTask;
        }

        // ------------------------------
        // DISLIKE
        // ------------------------------
        public Task DislikeEntryAsync(int id)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == id);
            if (entry != null)
                entry.Dislikes++;

            return Task.CompletedTask;
        }

        // ------------------------------
        // ADD COMMENT
        // ------------------------------
        public Task AddCommentAsync(int entryId, string author, string text)
        {
            var entry = _entries.FirstOrDefault(e => e.Id == entryId);
            if (entry != null)
            {
                if (entry.Comments == null)
                    entry.Comments = new List<Comment>();

                entry.Comments.Add(new Comment
                {
                    Author = author,
                    Text = text,
                    CreatedAt = DateTime.Now
                });
            }
            return Task.CompletedTask;
        }
    }
}
