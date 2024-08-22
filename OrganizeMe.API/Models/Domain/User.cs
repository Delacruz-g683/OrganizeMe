namespace OrganizeMe.API.Models;

public class User
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public ICollection<TodoItem?>? TodoItems { get; set; }
}