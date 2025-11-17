using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DailyNotes.Services;

public class EntryService
{
    private readonly List<Entry> entries = new();

    // Guarda una nueva entrada
    public Task SaveEntryAsync(Entry entry)
    {
        entries.Add(entry);
        return Task.CompletedTask;
    }

    // Obtiene TODAS las entradas
    public Task<List<Entry>> GetEntriesAsync()
    {
        return Task.FromResult(entries);
    }

    // Obtiene SOLO las entradas públicas
    public Task<List<Entry>> GetPublicEntriesAsync()
    {
        var publicEntries = entries
            .Where(e => e.IsPublic)
            .ToList();

        return Task.FromResult(publicEntries);
    }
}

public class Entry
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
