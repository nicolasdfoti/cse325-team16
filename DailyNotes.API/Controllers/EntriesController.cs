using DailyNotes.API.Models;
using DailyNotes.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DailyNotes.API.Controllers
{
    [ApiController]
    [Route("entries")]
    [Authorize]
    public class EntriesController : ControllerBase
    {
        private readonly EntryDbService _service;

        public EntriesController(EntryDbService service)
        {
            _service = service;
        }

        private string? GetUserIdNullable()
        {
            return User.FindFirst("id")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private string GetUserIdRequired()
        {
            return GetUserIdNullable()
                ?? throw new Exception("UserId not found in token.");
        }

        // GET: /entries
        [HttpGet]
        public async Task<ActionResult<List<Entry>>> GetMyEntriesAsync()
        {
            var userId = GetUserIdRequired();
            var entries = await _service.GetEntriesByUserIdAsync(userId);
            return Ok(entries);
        }

        // GET: /entries/{id}
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Entry>> GetEntryByIdAsync(string id)
        {
            var entry = await _service.GetByIdAsync(id);

            if (entry == null)
                return NotFound();

            if (entry.IsPublic)
                return Ok(entry);

            var userId = GetUserIdNullable();

            if (userId != null && entry.UserId == userId)
                return Ok(entry);

            return Unauthorized("This entry is private.");
        }

        // POST: /entries
        [HttpPost]
        public async Task<ActionResult<Entry>> Create(Entry entry)
        {
            var userId = GetUserIdRequired();
            entry.UserId = userId;
            entry.CreatedAt = DateTime.UtcNow;

            await _service.CreateAsync(entry);

            return Ok(entry);
        }

        // PUT: /entries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Entry updated)
        {
            var userId = GetUserIdRequired();

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (existing.UserId != userId)
                return Unauthorized("You can only edit your own entries.");

            updated.Id = id;
            updated.UserId = userId;
            updated.CreatedAt = existing.CreatedAt;
            updated.UpdatedAt = DateTime.UtcNow;
            updated.Comments = existing.Comments;
            updated.Likes = existing.Likes;

            await _service.UpdateAsync(updated);

            return Ok(updated);
        }

        // DELETE: /entries/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = GetUserIdRequired();

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (existing.UserId != userId)
                return Unauthorized("You can only delete your own entries.");

            await _service.DeleteAsync(id, userId);

            return Ok();
        }

        // GET: /entries/public
        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<List<Entry>>> GetPublicEntries()
        {
            var entries = await _service.GetPublicEntriesAsync();
            return Ok(entries);
        }

        // POST: /entries/{id}/like
        [HttpPost("{id}/like")]
        public async Task<IActionResult> AddLike(string id)
        {
            var entry = await _service.GetByIdAsync(id);

            if (entry == null)
                return NotFound();

            await _service.LikeAsync(id);

            var updated = await _service.GetByIdAsync(id);

            return Ok(updated);
        }

        // POST: /entries/{id}/comment
        [HttpPost("{id}/comment")]
        [Authorize]
        public async Task<IActionResult> AddComment(string id, [FromBody] AddCommentRequest req)
        {
            var entry = await _service.GetByIdAsync(id);
            if (entry == null)
                return NotFound();
                
            var userName = User.FindFirstValue(ClaimTypes.Name) ?? "Anonymous";

            await _service.AddCommentAsync(id, userName, req.Text);

            var updatedEntry = await _service.GetByIdAsync(id);
            return Ok(updatedEntry);
        }
    }
}
