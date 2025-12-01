namespace api.Models;

public class SpecialistService
{
    public int Id { get; set; }
    public int SpecialistId { get; set; }
    public int ServiceId { get; set; }
    public required decimal Cost { get; set; }
    public required TimeSpan Duration { get; set; }

    // navigation properties
    public Service Service { get; set;} = null!;
    public Specialist Specialist { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; } = new List<Reservation>();
}