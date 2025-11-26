using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using DailyNotes.Services;

namespace DailyNotes.Providers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthStateService _authStateService;

        public CustomAuthStateProvider(AuthStateService authStateService)
        {
            _authStateService = authStateService;
            _authStateService.OnChange += AuthStateChanged;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                await _authStateService.EnsureInitializedAsync();

                if (_authStateService.IsAuthenticated && _authStateService.CurrentUser != null)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, _authStateService.CurrentUser.Id),
                        new Claim(ClaimTypes.Name, _authStateService.CurrentUser.UserName),
                        new Claim(ClaimTypes.Email, _authStateService.CurrentUser.Email)
                    };

                    var identity = new ClaimsIdentity(claims, "jwt");
                    var user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }

                // Usuario no logueado
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AuthStateProvider error: {ex}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        private void AuthStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
