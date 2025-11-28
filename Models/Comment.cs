namespace DailyNotes.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int EntryId { get; set; }
        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; } = "";
        [Required(ErrorMessage = "Text is required.")]
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
