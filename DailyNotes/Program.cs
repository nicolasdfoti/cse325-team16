using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DailyNotes;
using DailyNotes.Models;
using DailyNotes.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:5146/")
});

builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<UserService>();

await builder.Build().RunAsync();
