using api.Enums;

namespace api.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; } //hash this eventually
    public required string Email { get; set; }
    public City? City { get; set; }

    //optional navigation properties
    public Specialist? Specialist { get; set; }
    public ICollection<Reservation> Reservations = new List<Reservation>();
} 