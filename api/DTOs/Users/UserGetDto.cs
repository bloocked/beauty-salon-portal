using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Users;

public class UserGetDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!; // suppress warning
    public string? Email { get; set; }
} 