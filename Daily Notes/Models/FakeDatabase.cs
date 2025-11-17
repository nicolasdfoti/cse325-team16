using System.Collections.Generic;
using Daily_Notes.Models;

namespace Daily_Notes.Models
{
    public static class FakeDatabase
    {
        public static List<Prompt> Prompts = new()
        {
            new Prompt { Id = 1, Text = "Describe algo por lo que estás agradecido hoy." },
            new Prompt { Id = 2, Text = "¿Qué aprendiste hoy?" }
        };

        public static List<JournalEntry> Entries = new()
        {
            new JournalEntry {
                Id = 1,
                Title = "Mi primer nota",
                Content = "Hoy fue un buen día.",
                CreatedAt = DateTime.Now.AddHours(-2),
                UserDisplayName = "Daniel",
                IsPublic = true
            }
        };
    }
}
