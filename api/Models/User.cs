using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    public required string Email { get; set; }
} 