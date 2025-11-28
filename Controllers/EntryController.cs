using DailyNotes.Models;
using DailyNotes.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyNotes.Controllers;

[ApiController]
[Route("Entries")]

public class EntryController : ControllerBase
{
    private readonly EntryService _entryService;

    public EntryController(EntryService entryService)
    {
        _entryService = entryService;
    }

    [HttpGet]
    public ActionResult<List<Entry>> GetAll() => _entryService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Entry> Get(string id)
    {
        var entry = _entryService.Get(id);

        if (entry == null)
            return NotFound();

        return entry;
    }

    [HttpPost]
    public IActionResult Create(Entry entry)
    {
        _entryService.Add(entry);
        return CreatedAtAction(nameof(Get), new { id = entry.EntryId }, entry);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, Entry entry)
    {
        if (id != entry.EntryId)
            return BadRequest();

        var existingEntry = _entryService.Get(id);
        if (existingEntry is null)
            return NotFound();

        _entryService.Update(entry);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var entry = _entryService.Get(id);

        if (entry is null)
            return NotFound();

        _entryService.Delete(id);

        return NoContent();
    }
}