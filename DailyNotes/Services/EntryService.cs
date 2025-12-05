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
                var response = await _http.GetAsync("/entries");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error fetching entries: {response.StatusCode}");
                    return new();
                }

                var result = await response.Content.ReadFromJsonAsync<List<Entry>>();
                return result ?? new();
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
            try
            {
                return await _http.GetFromJsonAsync<List<Entry>>("/entries/public") ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting public entries: {ex.Message}");
                return new List<Entry>();
            }
            finally
            {
                RemoveToken();
            }
        }

        public async Task<Entry?> AddCommentAsync(string entryId, string text)
        {
            try
        {
                var token = await _auth.GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    _http.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var payload = new { Text = text };

                var response = await _http.PostAsJsonAsync($"/entries/{entryId}/comment", payload);

                _http.DefaultRequestHeaders.Authorization = null;

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Entry>();
                }

                var err = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"AddCommentAsync failed: {response.StatusCode} - {err}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddCommentAsync exception: {ex}");

                _http.DefaultRequestHeaders.Authorization = null;
                return null;
            }
        }

        public async Task<Entry?> LikeEntryAsync(string id)
        {
            await AddTokenAsync();
            try
            {
                var response = await _http.PostAsync($"/entries/{id}/like", null);
                
                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<Entry>();
            }
            finally
            {
                RemoveToken();
            }
        }
    }
}