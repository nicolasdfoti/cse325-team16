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
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password are required.");

            try
            {
                var response = await _http.PostAsJsonAsync("/api/users/login", new LoginRequest(email, password));
                if (!response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>().ConfigureAwait(false);
                if (result != null)
                {
                    await _authStateService.LoginAsync(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Login failed: {ex.Message}");
                return null;
            }
        }

        public async Task<AuthResponse?> RegisterAsync(string userName, string email, int age, string password)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/users/register", new RegisterRequest(userName, email, age, password));
                if (!response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>().ConfigureAwait(false);
                if (result != null)
                {
                    await _authStateService.LoginAsync(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Registration failed: {ex.Message}");
                return null;
            }
        }
    }

    public record LoginRequest(string Email, string Password);
    public record RegisterRequest(string UserName, string Email, int Age, string Password);
}