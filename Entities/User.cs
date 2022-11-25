namespace Entities;

public class User
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }

    public User()
    {
    }

    public User(long id, string username, string email)
    {
        Id = id;
        Username = username;
        Email = email;
    }
}