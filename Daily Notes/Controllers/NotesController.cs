using Microsoft.AspNetCore.Mvc;

public class NotesController : Controller
{
    public IActionResult Index()
    {
        return View(FakeDatabase.Entries);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(JournalEntry entry)
    {
        entry.Id = FakeDatabase.Entries.Count + 1;
        entry.CreatedAt = DateTime.Now;
        entry.UserDisplayName = "Daniel"; 
        FakeDatabase.Entries.Add(entry);

        return RedirectToAction("Index");
    }
}
