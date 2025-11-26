using DailyNotes.Models;
using Microsoft.JSInterop;

namespace DailyNotes.Services
{
    public class AuthStateService
    {
        private readonly IJSRuntime _jsRuntime;
        private bool _isInitialized = false;

        public AuthStateService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public User? CurrentUser { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        public event Action? OnChange;

        public async Task EnsureInitializedAsync()
        {
            if (!_isInitialized)
            {
                await LoadFromLocalStorageAsync();
                _isInitialized = true;
            }
        }

        public async Task LoginAsync(AuthResponse authResponse)
        {
            Token = authResponse.Token;
            CurrentUser = new User
            {
                Id = authResponse.UserId,
                UserName = authResponse.UserName,
                Email = authResponse.Email
            };

            await SaveToLocalStorageAsync();
            NotifyStateChanged();
        }

        public async Task LogoutAsync()
        {
            Token = string.Empty;
            CurrentUser = null;

            await ClearLocalStorageAsync();
            NotifyStateChanged();
        }


        // ------------------------------
        // SAFE LOCAL STORAGE HELPERS
        // ------------------------------

        private async Task<string?> SafeGetItem(string key)
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            }
            catch
            {
                return null;
            }
        }

        private async Task LoadFromLocalStorageAsync()
        {
            try
            {
                var token = await SafeGetItem("authToken");
                if (!string.IsNullOrEmpty(token))
                {
                    Token = token;

                    CurrentUser = new User
                    {
                        Id = await SafeGetItem("userId") ?? "",
                        UserName = await SafeGetItem("userName") ?? "Guest",
                        Email = await SafeGetItem("userEmail") ?? ""
                    };

                    Console.WriteLine($"Loaded user from storage: {CurrentUser.UserName}");
                }
                else
                {
                    Console.WriteLine("No auth token found in storage");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading auth state: {ex}");
                await ClearLocalStorageAsync();
            }
        }

        private async Task SaveToLocalStorageAsync()
        {
            if (IsAuthenticated && CurrentUser != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", Token);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userName", CurrentUser.UserName);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userEmail", CurrentUser.Email);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userId", CurrentUser.Id);
            }
        }

        private async Task ClearLocalStorageAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userName");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userEmail");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userId");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing auth state: {ex}");
            }
        }

        public async Task<string> GetTokenAsync()
        {
            if (!_isInitialized)
                await EnsureInitializedAsync();

            return Token;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
