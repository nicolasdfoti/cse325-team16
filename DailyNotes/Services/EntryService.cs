using System.Net.Http.Headers;
using System.Net.Http.Json;
using DailyNotes.Models;

namespace DailyNotes.Services
{
    public class EntryService
    {
        private readonly HttpClient _http;
        private readonly AuthStateService _auth;

        public EntryService(HttpClient http, AuthStateService auth)
        {
            _http = http;
            _auth = auth;
        }

        private async Task AddTokenAsync()
        {
            var token = await _auth.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private void RemoveToken()
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<List<Entry>> GetMyEntriesAsync()
        {
            await AddTokenAsync();
            try
            {
                return await _http.GetFromJsonAsync<List<Entry>>("/entries") ?? new();
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task<Entry?> GetEntryByIdAsync(string id)
        {
            await AddTokenAsync();
            try
            {
                return await _http.GetFromJsonAsync<Entry>($"/entries/{id}");
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task SaveEntryAsync(Entry entry)
        {
            await AddTokenAsync();
            try
            {
                await _http.PostAsJsonAsync("/entries", entry);
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task UpdateEntryAsync(Entry entry)
        {
            await AddTokenAsync();
            try
            {
                await _http.PutAsJsonAsync($"/entries/{entry.Id}", entry);
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task DeleteEntryAsync(string id)
        {
            await AddTokenAsync();
            try
            {
                await _http.DeleteAsync($"/entries/{id}");
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task<List<Entry>> GetPublicEntriesAsync()
        {
            RemoveToken();
            try
            {
                return await _http.GetFromJsonAsync<List<Entry>>("/entries/public") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting public entries: {ex.Message}");
                return new List<Entry>();
            }
        }

        public async Task LikeEntryAsync(string id)
        {
            await AddTokenAsync();
            try
            {
                await _http.PostAsync($"/entries/{id}/like", null);
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task DislikeEntryAsync(string id)
        {
            await AddTokenAsync();
            try
            {
                await _http.PostAsync($"/entries/{id}/dislike", null);
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task AddCommentAsync(string entryId, string author, string text)
        {
            await AddTokenAsync();
            try
            {
                var comment = new { Author = author, Text = text };
                await _http.PostAsJsonAsync($"/entries/{entryId}/comments", comment);
            }
            finally
            {
                RemoveToken();
            }
        }
    }
}