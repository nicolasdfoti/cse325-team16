using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DailyNotes.Models
{
    public class Entry
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public string Prompt { get; set; } = "";

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters.")]
        public string Content { get; set; } = "";

        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        [Reguired]
        public bool IsPublic { get; set; }

        public int Likes { get; set; } = 0;

        public int Dislikes { get; set; } = 0;

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<Comment> Comments { get; set; } = new();
    }
}