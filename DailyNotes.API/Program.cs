using DailyNotes.API.Models;
using DailyNotes.API.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorDev", policy =>
        policy.WithOrigins("http://localhost:5146")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// MongoDB Settings
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
var mongoDatabaseName = builder.Configuration["DatabaseName"];

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConnectionString));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EntryService>();

var app = builder.Build();
app.UseCors("AllowBlazorDev");

// Test endpoint
app.MapGet("/", () => "DailyNotes API running!");

// Users endpoints
app.MapGet("/users", async (UserService service) =>
{
    return await service.GetAsync();
});

// Get all entries (my entries)
app.MapGet("/entries", async (EntryService service) =>
{
    var entries = await service.GetMyEntriesAsync();
    return Results.Ok(entries);
});

// Get public entries
app.MapGet("/entries/public", async (EntryService service) =>
{
    var entries = await service.GetPublicEntriesAsync();
    return Results.Ok(entries);
});

// Get entry by id
app.MapGet("/entries/{id}", async (EntryService service, string id) =>
{
    var entry = await service.GetEntryByIdAsync(id);
    return entry is not null ? Results.Ok(entry) : Results.NotFound();
});

// Create entry
app.MapPost("/entries", async (EntryService service, Entry entry) =>
{
    await service.CreateAsync(entry);
    return Results.Created($"/entries/{entry.Id}", entry);
});

// Update entry
app.MapPut("/entries/{id}", async (EntryService service, string id, Entry updated) =>
{
    updated.Id = MongoDB.Bson.ObjectId.Parse(id);
    await service.UpdateEntryAsync(updated);
    return Results.Ok(updated);
});

// Delete entry
app.MapDelete("/entries/{id}", async (EntryService service, string id) =>
{
    await service.DeleteEntryAsync(id);
    return Results.Ok();
});

// Like entry
app.MapPost("/entries/{id}/like", async (EntryService service, string id) =>
{
    await service.LikeEntryAsync(id);
    return Results.Ok();
});

// Dislike entry
app.MapPost("/entries/{id}/dislike", async (EntryService service, string id) =>
{
    await service.DislikeEntryAsync(id);
    return Results.Ok();
});

// Add comment
app.MapPost("/entries/{id}/comments", async (EntryService service, string id, Comment comment) =>
{
    await service.AddCommentAsync(id, comment.Author, comment.Text);
    return Results.Ok();
});

app.MapPost("/users", async (UserService service, User user) =>
{
    await service.CreateAsync(user);
    return Results.Created($"/users/{user.Id}", user);
});

app.Run();
