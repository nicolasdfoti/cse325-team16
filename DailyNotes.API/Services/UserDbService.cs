using DailyNotes.API.Models;
using MongoDB.Driver;
using BCrypt.Net;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DailyNotes.API.Services
{
    public class UserDbService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IConfiguration _config;

        public UserDbService(IMongoDatabase db, IConfiguration config)
        {
            _users = db.GetCollection<User>("Users");
            _config = config;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        // -------------------------
        // REGISTER
        // -------------------------
        public async Task<RegisterResult> RegisterAsync(UserRegisterDto dto)
        {
            var existing = await GetByEmailAsync(dto.Email);
            if (existing != null)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = "Email already exists",
                    UserId = null
                };
            }

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _users.InsertOneAsync(user);

            return new RegisterResult
            {
                Success = true,
                Message = "User registered!",
                UserId = user.Id
            };
        }

        // -------------------------
        // LOGIN
        // -------------------------
        public async Task<string?> LoginAsync(UserLoginDto dto)
        {
            var user = await GetByEmailAsync(dto.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            return GenerateJwt(user);
        }

        // -------------------------
        // JWT GENERATION
        // -------------------------
        private string GenerateJwt(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
