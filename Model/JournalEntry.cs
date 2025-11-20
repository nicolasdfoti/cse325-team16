namespace JournalApp.Model;

public class JournalEntry
{
    public int JournalEntryId { get; set; }

    public string UserId { get; set; }

    public DateTime CreatedTime { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string Prompt { get; set; }
}