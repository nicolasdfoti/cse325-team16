namespace Daily_Notes.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }          // nullable
        public string? Email { get; set; }             // nullable
        public string? PasswordHash { get; set; }      // nullable

        public ICollection<JournalEntry> JournalEntries { get; set; }
            = new List<JournalEntry>();                // inicializado
    }
}
