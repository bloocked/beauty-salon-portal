namespace api.Models;

public class Specialist
{
    public int Fk_UserId { get; set; } 
    public int Fk_SalonId { get; set; }

    // navigation properties
    public required User User { get; set; }
}