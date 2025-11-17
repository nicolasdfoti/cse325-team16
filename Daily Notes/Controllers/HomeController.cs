using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var prompt = FakeDatabase.Prompts.First();
        var publicEntries = FakeDatabase.Entries.Where(e => e.IsPublic).ToList();

        ViewBag.Prompt = prompt;
        return View(publicEntries);
    }
}
