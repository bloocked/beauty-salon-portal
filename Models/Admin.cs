namespace api.Models;

public class Admin
{
    public int Id { get; set; }
    public int UserId { get; set; }

    // nav properties
    public User User { get; set; } = null!;
}