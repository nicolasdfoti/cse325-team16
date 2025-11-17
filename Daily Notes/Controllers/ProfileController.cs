using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    public IActionResult Index()
    {
        var entries = FakeDatabase.Entries
            .Where(e => e.UserDisplayName == "Daniel")
            .ToList();
        
        return View(entries);
    }
}
