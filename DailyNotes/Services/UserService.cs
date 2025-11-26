using DailyNotes.Models;
using System.Net.Http.Json;

namespace DailyNotes.Services
{
    public class UserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                var allUsers = await _http.GetFromJsonAsync<List<User>>("/users");
                return allUsers?.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        public async Task CreateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            await _http.PostAsJsonAsync("/users", user);
        }
    }   
}