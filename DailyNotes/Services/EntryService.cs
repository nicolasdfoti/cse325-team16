using System.Net.Http.Json;
using DailyNotes.Models;

namespace DailyNotes.Services
{
    public class EntryService
    {
        private readonly HttpClient _http;
        public EntryService(HttpClient http) => _http = http;

        public async Task<List<Entry>> GetMyEntriesAsync()
        {
            return await _http.GetFromJsonAsync<List<Entry>>("api/entries/mine");
        }

        public async Task<Entry> GetEntryByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Entry>($"api/entries/{id}");
        }

        public async Task UpdateEntryAsync(Entry entry)
        {
            await _http.PutAsJsonAsync($"api/entries/{entry.Id}", entry);
        }

        public async Task DeleteEntryAsync(int id)
        {
            await _http.DeleteAsync($"api/entries/{id}");
        }

        public async Task SaveEntryAsync(Entry entry)
        {
            await _http.PostAsJsonAsync("api/entries", entry);
        }

        public async Task<List<Entry>> GetPublicEntriesAsync()
        {
            return await _http.GetFromJsonAsync<List<Entry>>("api/entries/public");
        }

        public async Task LikeEntryAsync(int id)
        {
            await _http.PostAsync($"api/entries/{id}/like", null);
        }

        public async Task DislikeEntryAsync(int id)
        {
            await _http.PostAsync($"api/entries/{id}/dislike", null);
        }

        public async Task AddCommentAsync(int entryId, string author, string text)
        {
            await _http.PostAsJsonAsync($"api/entries/{entryId}/comments", new { author, text });
        }
    }
}
