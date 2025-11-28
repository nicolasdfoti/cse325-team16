namespace DailyNotes.Model;

public class User
{
    public string UserId { get; set; }
    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; set; }
    public bool IsAdmin { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public string PasswordHash { get; set; }
}