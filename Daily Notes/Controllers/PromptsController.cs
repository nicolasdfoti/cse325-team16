using Microsoft.AspNetCore.Mvc;

public class PromptsController : Controller
{
    public IActionResult Index()
    {
        return View(FakeDatabase.Prompts);
    }
}
