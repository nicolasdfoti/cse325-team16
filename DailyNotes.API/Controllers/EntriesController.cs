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

        private string GetUserId()
        {
            return User.FindFirst("id")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new Exception("UserId not found in token.");
        }

        // GET /entries
        [HttpGet]
        public async Task<ActionResult<List<Entry>>> GetMyEntriesAsync()
        {
            var userId = GetUserId();
            var entries = await _service.GetEntriesByUserIdAsync(userId);
            return Ok(entries);
        }

        // GET /entries/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Entry>> GetEntryByIdAsync(string id)
        {
            var userId = GetUserId();
            var entry = await _service.GetByIdAsync(id);

            if (entry == null || entry.UserId != userId)
                return NotFound();

            return Ok(entry);
        }

        // POST /entries
        [HttpPost]
        public async Task<ActionResult<Entry>> Create(Entry entry)
        {
            var userId = GetUserId();
            entry.UserId = userId;
            entry.CreatedAt = DateTime.UtcNow;

            await _service.CreateAsync(entry);
            return Ok(entry);
        }

        // PUT /entries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Entry updated)
        {
            var userId = GetUserId();

            var existing = await _service.GetByIdAsync(id);
            if (existing == null || existing.UserId != userId)
                return NotFound();

            updated.Id = id;
            updated.UserId = userId;

            await _service.UpdateAsync(updated);
            return Ok(updated);
        }

        // DELETE /entries/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = GetUserId();

            var existing = await _service.GetByIdAsync(id);
            if (existing == null || existing.UserId != userId)
                return NotFound();

            await _service.DeleteAsync(id, userId);
            return Ok();
        }

        // GET /entries/public
        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<List<Entry>>> GetPublicEntries()
        {
            var entries = await _service.GetPublicEntriesAsync();
            return Ok(entries);
        }
    }
}
