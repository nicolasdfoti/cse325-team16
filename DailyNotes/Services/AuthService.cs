using System.Net.Http.Json;
using DailyNotes.Models;

namespace DailyNotes.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly AuthStateService _authStateService;
        
        public AuthService(HttpClient http, AuthStateService authStateService)
        {
            _http = http;
            _authStateService = authStateService;
        }

        public async Task<AuthResponse?> LoginAsync(string email, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/users/login", new
                {
                    Email = email,
                    Password = password
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result != null)
                    {
                        await _authStateService.LoginAsync(result);
                    }
                    return result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<AuthResponse?> RegisterAsync(string userName, string email, int age, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/users/register", new
                {
                    UserName = userName,
                    Email = email,
                    Age = age,
                    Password = password
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    if (result != null)
                    {
                        await _authStateService.LoginAsync(result);
                    }
                    return result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}