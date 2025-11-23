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
            var allUsers = await _http.GetFromJsonAsync<List<User>>("http://localhost:5147/users");
            if (allUsers == null) return null;
            return allUsers.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        public async Task CreateAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            await _http.PostAsJsonAsync("http://localhost:5147/users", user);
        }
    }   
}
