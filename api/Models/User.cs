using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; } //hash this eventually
    public required string Email { get; set; }
} 