using System;
using System.Collections.Generic;

namespace DailyNotes.Models
{
    public class Entry
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Content { get; set; } = "";

        public bool IsPublic { get; set; }

        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }   // ← FALTABA ESTA PROPIEDAD

        public List<Comment> Comments { get; set; } = new();
    }
}
