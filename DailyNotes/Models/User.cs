namespace DailyNotes.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string PasswordHash { get; set; }
    }
}
