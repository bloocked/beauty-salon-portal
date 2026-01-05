using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Users;

public class UserCreateDto
{
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
} 