using System;
using System.ComponentModel.DataAnnotations;

namespace Daily_Notes.Models
{
    public class JournalEntry
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsPublic { get; set; } = false;

        // IMPORTANTE PARA FAKE DATABASE
        public string? UserDisplayName { get; set; }
    }
}
