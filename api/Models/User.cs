using api.Enums;

namespace api.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }

    //optional navigation properties
    public Specialist? Specialist { get; set; }
    public Admin? Admin { get; set; }
    public ICollection<Reservation> Reservations = new List<Reservation>();
} 