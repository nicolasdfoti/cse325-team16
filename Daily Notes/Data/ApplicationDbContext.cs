using Daily_Notes.Models;

namespace Daily_Notes.Data
{
    public class ApplicationDbContext
    {
        public List<JournalEntry> JournalEntries { get; set; } = new();
        public List<User> Users { get; set; } = new();
    }
}
