using DailyNotes.Models;
using DailyNotes.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyNotes.Controllers;

[ApiController]
[Route("Users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(string id)
    {
        var user = _userService.Get(id);

        if (user == null)
            return NotFound();

        return user;
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        _userService.Add(user);
        return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
    }

    // 🔑 Login route
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _userService.Authenticate(request.Username, request.Password);

        if (user == null)
            return Unauthorized(new { message = "Invalid username or password" });

        // You could return a JWT or session token here
        return Ok(new
        {
            message = "Login successful",
            user = new { user.UserId, user.Username, user.Email }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, User user)
    {
        if (id != user.UserId)
            return BadRequest();

        var existingUser = _userService.Get(id);
        if (existingUser is null)
            return NotFound();

        _userService.Update(user);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var user = _userService.Get(id);

        if (user is null)
            return NotFound();

        _userService.Delete(id);

        return NoContent();
    }
}

// DTO for login request
public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}