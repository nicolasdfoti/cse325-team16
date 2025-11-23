using DailyNotes.API.Models;
using MongoDB.Driver;

namespace DailyNotes.API.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        public async Task<List<User>> GetAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task CreateAsync(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await _users.InsertOneAsync(user);
        }
    }
}
