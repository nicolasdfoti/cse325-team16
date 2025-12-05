using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DailyNotes;
using DailyNotes.Services;
using DailyNotes.Providers;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

var host = builder.Build();

var authState = host.Services.GetRequiredService<AuthStateService>();
await authState.EnsureInitializedAsync();

await host.RunAsync();