using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Users;

public class RegisterUserDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    public required string Email { get; set; }
} 