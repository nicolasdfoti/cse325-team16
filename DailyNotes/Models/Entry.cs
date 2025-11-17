namespace DailyNotes.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
    }
}
